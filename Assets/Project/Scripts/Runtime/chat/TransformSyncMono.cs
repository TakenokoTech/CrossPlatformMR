using Photon.Pun;
using Photon.Realtime;
using Project.Scripts.Runtime.utils;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation
namespace Project.Scripts.Runtime.chat
{
    public class TransformSyncMono : MonoBehaviourPunCallbacks
    {
        private const string Tag = "ApiBuilder";

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        void OnGUI()
        {
            GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
        }


        public override void OnConnectedToMaster()
        {
            Log.D(Tag, "OnConnectedToMaster");
            PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            Log.D(Tag, "OnJoinedRoom");
            var position = new Vector3(0,0, 5);
            var monster = PhotonNetwork.Instantiate("SyncObject", position, Quaternion.identity, 0);
            var monsterScript = monster.GetComponent<SyncObjectScript>();
            monsterScript.enabled = true;
        }
    }
}
