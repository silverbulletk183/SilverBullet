using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;


using UnityEngine;
using System.Linq;


/// <summary>
/// Một ví dụ đơn giản kiểu "Hello World" sử dụng Lobby, áp dụng hầu hết các API của Lobby.
///
/// CÀI ĐẶT:
///  1. Đính kèm script này vào một GameObject trong scene
///  2. Đăng nhập vào tài khoản Unity developer của bạn
///  3. Liên kết dự án của bạn với một dự án cloud đã kích hoạt Lobby
///  4. Chạy chế độ play và quan sát các log trong debug
///
/// </summary>
public class LobbyHelloWorld : MonoBehaviour
{
    // Thuộc tính Inspector với các giá trị ban đầu

    /// <summary>
    /// Sử dụng để đặt tên lobby trong ví dụ này.
    /// </summary>
    public string newLobbyName = "LobbyHelloWorld" + Guid.NewGuid();

    /// <summary>
    /// Sử dụng để đặt số lượng người chơi tối đa trong ví dụ này.
    /// </summary>
    public int maxPlayers = 8;

    /// <summary>
    /// Sử dụng để xác định xem lobby có phải là riêng tư hay không trong ví dụ này.
    /// </summary>
    public bool isPrivate = false;

    // Chúng ta sẽ chỉ tham gia một lobby tại một thời điểm cho demo này, vì vậy sẽ theo dõi nó ở đây
    private Lobby currentLobby;

    async void Start()
    {
        try
        {
            await ExecuteLobbyDemoAsync();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        await CleanupDemoLobbyAsync();

        Debug.Log("Hoàn thành demo!");
    }

    // Dọn dẹp lobby chúng ta đã tham gia nếu chúng ta là chủ phòng
    async Task CleanupDemoLobbyAsync()
    {
        var localPlayerId = AuthenticationService.Instance.PlayerId;

        // Điều này là để không để lại các lobby bị bỏ trống nếu demo bị lỗi giữa chừng
        if (currentLobby != null && currentLobby.HostId.Equals(localPlayerId))
        {
            await LobbyService.Instance.DeleteLobbyAsync(currentLobby.Id);
            Debug.Log($"Đã xóa lobby {currentLobby.Name} ({currentLobby.Id})");
        }
    }

    // Một demo cơ bản về chức năng của lobby
    async Task ExecuteLobbyDemoAsync()
    {
        await UnityServices.InitializeAsync();

        // Đăng nhập một người chơi cho client game này
        Player loggedInPlayer = await GetPlayerFromAnonymousLoginAsync();

        // Thêm một số dữ liệu vào người chơi của chúng ta
        // Dữ liệu này sẽ được bao gồm trong lobby dưới phần players -> player.data
        loggedInPlayer.Data.Add("Ready", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "No"));

        // Truy vấn các lobby hiện có

        // Sử dụng bộ lọc để chỉ trả về các lobby khớp với các điều kiện cụ thể
        // Bạn chỉ có thể lọc dựa trên các thuộc tính tích hợp sẵn (Ví dụ: AvailableSlots) hoặc dữ liệu tùy chỉnh được đánh chỉ mục (S1, N1, v.v.)
        List<QueryFilter> queryFilters = new List<QueryFilter>
        {
            // Tìm kiếm các trò chơi có chỗ trống (AvailableSlots lớn hơn 0)
            new QueryFilter(
                field: QueryFilter.FieldOptions.AvailableSlots,
                op: QueryFilter.OpOptions.GT,
                value: "0"),

            // Thêm các bộ lọc cho các trường dữ liệu tùy chỉnh được đánh chỉ mục
            new QueryFilter(
                field: QueryFilter.FieldOptions.S1, // S1 = "Test"
                op: QueryFilter.OpOptions.EQ,
                value: "true"),

            new QueryFilter(
                field: QueryFilter.FieldOptions.S2, // S2 = "GameMode"
                op: QueryFilter.OpOptions.EQ,
                value: "ctf"),

            // Bộ lọc phạm vi kỹ năng (skill) tùy chỉnh (skill là trường dữ liệu số tùy chỉnh trong ví dụ này)
            new QueryFilter(
                field: QueryFilter.FieldOptions.N1, // N1 = "Skill"
                op: QueryFilter.OpOptions.GT,
                value: "0"),

            new QueryFilter(
                field: QueryFilter.FieldOptions.N1, // N1 = "Skill"
                op: QueryFilter.OpOptions.LT,
                value: "51"),
        };

        // Kết quả truy vấn cũng có thể được sắp xếp
        List<QueryOrder> queryOrdering = new List<QueryOrder>
        {
            new QueryOrder(true, QueryOrder.FieldOptions.AvailableSlots),
            new QueryOrder(false, QueryOrder.FieldOptions.Created),
            new QueryOrder(false, QueryOrder.FieldOptions.Name),
        };

        // Gọi API Query
        QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(new QueryLobbiesOptions()
        {
            Count = 20, // Ghi đè số lượng kết quả trả về mặc định
            Filters = queryFilters,
            Order = queryOrdering,
        });

        List<Lobby> foundLobbies = response.Results;

        if (foundLobbies.Any()) // Thử tham gia một lobby ngẫu nhiên nếu tìm thấy
        {
            // In thông tin về các lobby đã tìm thấy
            Debug.Log("Tìm thấy các lobby:\n" + JsonConvert.SerializeObject(foundLobbies));

            // Chọn một lobby ngẫu nhiên để tham gia
            var randomLobby = foundLobbies[UnityEngine.Random.Range(0, foundLobbies.Count)];

            // Thử tham gia vào lobby
            currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(
                lobbyId: randomLobby.Id,
                options: new JoinLobbyByIdOptions()
                {
                    Player = loggedInPlayer
                });

            Debug.Log($"Đã tham gia vào lobby {currentLobby.Name} ({currentLobby.Id})");
        }
        else // Không tìm thấy lobby nào, tạo một lobby mới
        {
            var lobbyData = new Dictionary<string, DataObject>()
            {
                ["Test"] = new DataObject(DataObject.VisibilityOptions.Public, "true", DataObject.IndexOptions.S1),
                ["GameMode"] = new DataObject(DataObject.VisibilityOptions.Public, "ctf", DataObject.IndexOptions.S2),
                ["Skill"] = new DataObject(DataObject.VisibilityOptions.Public, UnityEngine.Random.Range(1, 51).ToString(), DataObject.IndexOptions.N1),
                ["Rank"] = new DataObject(DataObject.VisibilityOptions.Public, UnityEngine.Random.Range(1, 51).ToString()),
            };

            currentLobby = await LobbyService.Instance.CreateLobbyAsync(
                lobbyName: newLobbyName,
                maxPlayers: maxPlayers,
                options: new CreateLobbyOptions()
                {
                    Data = lobbyData,
                    IsPrivate = isPrivate,
                    Player = loggedInPlayer
                });

            Debug.Log($"Đã tạo một lobby mới {currentLobby.Name} ({currentLobby.Id})");
        }

        // Viết một chút thông tin về lobby mà chúng ta đã tham gia / tạo
        Debug.Log("Thông tin lobby:\n" + JsonConvert.SerializeObject(currentLobby));
    }

    // Đăng nhập người chơi bằng API "Anonymous Login" của Unity và tạo một đối tượng Player để sử dụng với API Lobbies
    static async Task<Player> GetPlayerFromAnonymousLoginAsync()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log($"Đang cố gắng đăng nhập người chơi...");

            // Sử dụng Unity Authentication để đăng nhập
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                throw new InvalidOperationException("Người chơi chưa đăng nhập thành công; không thể tiếp tục mà không có người chơi đăng nhập");
            }
        }

        Debug.Log("Người chơi đã đăng nhập với ID: " + AuthenticationService.Instance.PlayerId);

        return new Player(AuthenticationService.Instance.PlayerId, null, data: new Dictionary<string, PlayerDataObject>());
    }
}
