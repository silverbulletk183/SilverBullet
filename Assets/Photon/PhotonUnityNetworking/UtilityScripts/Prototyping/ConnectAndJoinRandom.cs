using UnityEngine;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    public class ConnectAndJoinRandom : MonoBehaviourPunCallbacks
    {
        public bool AutoConnect = true;
        public static ConnectAndJoinRandom Instance {  get; private set; }

        public byte Version = 1;

        // Ch?n ID phòng tr?c ti?p (1 ho?c 2)
        public string roomVoiceChatID; // B?n có th? thay ??i thành "2" n?u mu?n tham gia phòng 2

        [Tooltip("The max number of players allowed in room.")]
        public byte MaxPlayers = 4;

        public int playerTTL = -1;
        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            
            Debug.Log("roomvioce chat id" + roomVoiceChatID);
           
        }

        public void ConnectNow()
        {
            Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server.");
            JoinSpecificRoom();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby(). This client is now connected to Relay.");
            JoinSpecificRoom();
        }

        private void JoinSpecificRoom()
        {
            Debug.Log("Attempting to join room: " + roomVoiceChatID);
            PhotonNetwork.JoinRoom(roomVoiceChatID); // Tham gia phòng theo ID ?ã ch? ??nh
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed() was called by PUN. Room not found, creating room: " + roomVoiceChatID);
            RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
            if (playerTTL >= 0)
                roomOptions.PlayerTtl = playerTTL;

            PhotonNetwork.CreateRoom(roomVoiceChatID, roomOptions, null); // T?o phòng m?i n?u không tìm th?y
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in room: " + PhotonNetwork.CurrentRoom.Name);
            // B?t ??u trò ch?i ho?c th?c hi?n các hành ??ng khác ? ?ây
        }
        public void setRoomID(string idRoom)
        {
            roomVoiceChatID = idRoom;
            if (this.AutoConnect)
            {
                this.ConnectNow();
            }
        }
    }
    
}