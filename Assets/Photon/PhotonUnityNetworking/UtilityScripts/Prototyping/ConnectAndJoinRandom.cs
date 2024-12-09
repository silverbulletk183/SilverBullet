using UnityEngine;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    public class ConnectAndJoinRandom : MonoBehaviourPunCallbacks
    {
        public bool AutoConnect = true;
        public static ConnectAndJoinRandom Instance {  get; private set; }

        public byte Version = 1;

        // Ch?n ID ph�ng tr?c ti?p (1 ho?c 2)
        public string roomVoiceChatID; // B?n c� th? thay ??i th�nh "2" n?u mu?n tham gia ph�ng 2

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
            PhotonNetwork.JoinRoom(roomVoiceChatID); // Tham gia ph�ng theo ID ?� ch? ??nh
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed() was called by PUN. Room not found, creating room: " + roomVoiceChatID);
            RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
            if (playerTTL >= 0)
                roomOptions.PlayerTtl = playerTTL;

            PhotonNetwork.CreateRoom(roomVoiceChatID, roomOptions, null); // T?o ph�ng m?i n?u kh�ng t�m th?y
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in room: " + PhotonNetwork.CurrentRoom.Name);
            // B?t ??u tr� ch?i ho?c th?c hi?n c�c h�nh ??ng kh�c ? ?�y
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