using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamCreationUIManager : NetworkBehaviour
{
    // Các thành phần giao diện người dùng
    public TextMeshProUGUI statusText; // Phần tử văn bản để hiển thị trạng thái số lượng người chơi đã kết nối và sẵn sàng
    public List<GameObject> listAvt = new List<GameObject>(); // Danh sách các đối tượng GameObject đại diện cho avatar
    public Button readyButton; // Nút để người chơi bấm khi đã sẵn sàng
    public Button btnBack;
    public TextMeshProUGUI txtIDRoom;

    // Các biến mạng để theo dõi số lượng client đã kết nối và số lượng client đã sẵn sàng
    private NetworkVariable<int> connectedClients = new NetworkVariable<int>();
    private NetworkVariable<int> readyClients = new NetworkVariable<int>();

    // Từ điển để theo dõi trạng thái sẵn sàng và avatar được gán cho mỗi người chơi (theo clientId của họ)
    private Dictionary<ulong, bool> playerReadiness = new Dictionary<ulong, bool>(); // Lưu trạng thái sẵn sàng của mỗi người chơi
    private Dictionary<ulong, int> playerAvatars = new Dictionary<ulong, int>(); // Lưu chỉ số avatar cho mỗi người chơi



    private void Start()
    {
        // Thiết lập nút "Sẵn sàng" để gọi hàm OnReadyButtonClicked khi được nhấn
        readyButton.onClick.AddListener(OnReadyButtonClicked);


        // Nếu là server, đăng ký callback khi client kết nối hoặc ngắt kết nối
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        // Đăng ký theo dõi sự thay đổi số lượng client đã kết nối và sẵn sàng để cập nhật trạng thái UI
        connectedClients.OnValueChanged += UpdateConnectionCount;
        readyClients.OnValueChanged += UpdateReadyCount;

        btnBack.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            Debug.Log("dss");
            Loader.Load(Loader.Scene.mainHomecp);
        });
       Lobby lobby= SilverBulletGameLobby.Instance.GetLobby();
  //      txtIDRoom.text = lobby.LobbyCode;
    }

    public override void OnNetworkSpawn()
    {
        // Khi đối tượng được tạo ra trong mạng, server sẽ thiết lập các giá trị ban đầu
        if (IsServer)
        {
            // Khởi tạo số lượng client đã kết nối
            connectedClients.Value = NetworkManager.Singleton.ConnectedClientsList.Count;
            playerReadiness[NetworkManager.Singleton.LocalClientId] = false; // Ban đầu, host không sẵn sàng

            // Gán avatar cho host (client server)
            AssignAvatar(NetworkManager.Singleton.LocalClientId);
        }

        // Ẩn tất cả các avatar ban đầu, chúng chỉ xuất hiện khi người chơi sẵn sàng
        foreach (var avatar in listAvt)
        {
            avatar.SetActive(false);
        }

        // Cập nhật văn bản trạng thái hiển thị (số lượng người chơi đã kết nối và sẵn sàng)
        UpdateStatus();
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            connectedClients.Value++;
            playerReadiness[clientId] = false;
            AssignAvatar(clientId);

            // Ensure the new player sees the correct avatars and readiness state of all players
            foreach (var kvp in playerAvatars)
            {
                bool isReady = playerReadiness.ContainsKey(kvp.Key) ? playerReadiness[kvp.Key] : false;
                UpdateAvatarStatusClientRpc(kvp.Key, kvp.Value, isReady);
            }
        }
    }


    private void OnClientDisconnected(ulong clientId)
    {
        // Khi một client ngắt kết nối, nếu là server:
        if (IsServer)
        {
            connectedClients.Value--; // Giảm số lượng client đã kết nối
            // Nếu client ngắt kết nối đã sẵn sàng, giảm số lượng client đã sẵn sàng
            if (playerReadiness.ContainsKey(clientId) && playerReadiness[clientId])
            {
                readyClients.Value--;
            }

            // Xóa client đã ngắt kết nối khỏi danh sách theo dõi trạng thái sẵn sàng và avatar
            playerReadiness.Remove(clientId);
            playerAvatars.Remove(clientId); // Xóa avatar đã được gán
        }
    }

    private void AssignAvatar(ulong clientId)
    {
        // Gán một chỉ số avatar cho người chơi (theo thứ tự dựa trên số lượng client đã kết nối)
        int avatarIndex = connectedClients.Value - 1;
        playerAvatars[clientId] = avatarIndex; // Lưu chỉ số avatar cho người chơi

        // Đồng bộ trạng thái avatar (ban đầu là ẩn) với tất cả các client
        UpdateAvatarStatusClientRpc(clientId, avatarIndex, false); // Avatar sẽ bị ẩn cho đến khi người chơi sẵn sàng
    }

    private void UpdateConnectionCount(int oldValue, int newValue)
    {
        // Khi số lượng client đã kết nối thay đổi, cập nhật trạng thái hiển thị
        UpdateStatus();
    }

    private void UpdateReadyCount(int oldValue, int newValue)
    {
        // Khi số lượng client đã sẵn sàng thay đổi, cập nhật trạng thái hiển thị
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        // Cập nhật văn bản trạng thái với số lượng người chơi đã kết nối và sẵn sàng
        statusText.text = $"Người chơi đã kết nối: {connectedClients.Value}, Sẵn sàng: {readyClients.Value}";

        // Nếu tất cả người chơi đã kết nối đều sẵn sàng và có ít nhất một người chơi đã kết nối, bắt đầu trò chơi
        if (IsServer && readyClients.Value == connectedClients.Value && connectedClients.Value > 0)
        {
            GoToGameScreen(); // Chuyển đến cảnh game
        }
    }

    public void OnReadyButtonClicked()
    {
        // Khi nút "Sẵn sàng" được nhấn, chuyển đổi trạng thái sẵn sàng của người chơi
        ToggleReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        // Hàm phía server để xử lý chuyển đổi trạng thái sẵn sàng
        ulong clientId = serverRpcParams.Receive.SenderClientId; // Lấy clientId của người chơi đã nhấn "Sẵn sàng"
        if (!playerReadiness.ContainsKey(clientId))
        {
            playerReadiness[clientId] = false; // Khởi tạo trạng thái sẵn sàng là false nếu chưa được theo dõi
        }

        // Chuyển đổi trạng thái sẵn sàng
        bool newReadyState = !playerReadiness[clientId];
        playerReadiness[clientId] = newReadyState;

        // Cập nhật số lượng client sẵn sàng dựa trên trạng thái mới
        if (newReadyState)
        {
            readyClients.Value++;
        }
        else
        {
            readyClients.Value--;
        }

        // Đồng bộ trạng thái sẵn sàng của người chơi với tất cả các client
        UpdateReadyStatusClientRpc(clientId, newReadyState);
    }

    [ClientRpc]
    private void UpdateReadyStatusClientRpc(ulong clientId, bool isReady)
    {
        // Hàm phía client để cập nhật giao diện và hiển thị avatar dựa trên trạng thái sẵn sàng
        if (playerAvatars.ContainsKey(clientId))
        {
            int avatarIndex = playerAvatars[clientId]; // Lấy chỉ số avatar của người chơi
            listAvt[avatarIndex].SetActive(isReady); // Hiển thị avatar nếu người chơi sẵn sàng, ẩn nếu không
        }

        // Cập nhật văn bản nút "Sẵn sàng" cho người chơi cục bộ
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = isReady ? "Hủy" : "Sẵn sàng";
        }
    }

    [ClientRpc]
    private void UpdateAvatarStatusClientRpc(ulong clientId, int avatarIndex, bool isReady)
    {
        // Ensure the avatar is assigned
        if (!playerAvatars.ContainsKey(clientId))
        {
            playerAvatars[clientId] = avatarIndex;
        }

        // Set the avatar active or inactive based on the ready status
        listAvt[avatarIndex].SetActive(isReady);
    }


    private void GoToGameScreen()
    {
        // Hàm server để tải cảnh game khi tất cả người chơi đã sẵn sàng
      //  SilverBulletGameLobby.Instance.DeleteLobby();
        Loader.LoadNetwork(Loader.Scene.SampleScene);
    }
}
