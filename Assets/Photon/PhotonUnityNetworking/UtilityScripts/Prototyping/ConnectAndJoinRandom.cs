using UnityEngine;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    public class ConnectAndJoinRandom : MonoBehaviourPunCallbacks
    {
        public static ConnectAndJoinRandom Instance { get; private set; }

        [Header("Connection Settings")]
        public bool AutoConnect = true;
        public byte Version = 1;

        [Header("Room Settings")]
        public string roomVoiceChatID; // ID của phòng sẽ tham gia
        [Tooltip("The max number of players allowed in room.")]
        public int MaxPlayers = 4;
        public int playerTTL = -1;

        private bool isConnecting = false;

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
          //  DontDestroyOnLoad(gameObject); // Đảm bảo đối tượng không bị hủy khi chuyển cảnh
        }
       
        private void Update()
        {
            // Kiểm tra xem đã có roomVoiceChatID hay chưa và bắt đầu kết nối nếu chưa kết nối
            if (!string.IsNullOrEmpty(roomVoiceChatID) && !isConnecting&&MaxPlayers>=6)
            {
                StartConnect();
            }
        }

        public void SetRoomID(string idRoom)
        {
            roomVoiceChatID = idRoom;
           // Debug.Log($"Room ID set to: {roomVoiceChatID}");
        }
        public void SetMaxPlayer(int _maxPlayer)
        {
            MaxPlayers = _maxPlayer;
        }
        public void StartConnect()
        {
            if (AutoConnect && !isConnecting)
            {
                isConnecting = true;
                Debug.Log("Starting connection to Photon...");
                ConnectNow();
            }
        }

        private void ConnectNow()
        {
            Debug.Log("Calling PhotonNetwork.ConnectUsingSettings()...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = $"{Version}.{SceneManagerHelper.ActiveSceneBuildIndex}";
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master Server. Attempting to join specific room...");
            JoinSpecificRoom();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby. Attempting to join specific room...");
            JoinSpecificRoom();
        }

        private void JoinSpecificRoom()
        {
            Debug.Log($"Attempting to join room: {roomVoiceChatID}");
            PhotonNetwork.JoinRoom(roomVoiceChatID);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogWarning($"Failed to join room: {message}. Creating room: {roomVoiceChatID}");
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = this.MaxPlayers,
                PlayerTtl = playerTTL >= 0 ? playerTTL : 0 // Nếu không đặt TTL, để mặc định là 0
            };

            PhotonNetwork.CreateRoom(roomVoiceChatID, roomOptions, null);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogError($"Disconnected from Photon: {cause}");
            isConnecting = false;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"Successfully joined room: {PhotonNetwork.CurrentRoom.Name}");
            isConnecting = false;
            // Thực hiện các hành động khi vào phòng (VD: Bắt đầu trò chơi)
        }
    }
}
