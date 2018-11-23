using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : Photon.PunBehaviour {

    public GameObject[] startPositions;
    public static int[] SpaceWay = { -1, -1 };
    [SerializeField]
    Ball ball;
    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings("WOTv1.0");
	}
	
    public override void OnConnectedToMaster()
    {
        print("正在接入大廳");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {        
        RoomOptions options = new RoomOptions
        {
                MaxPlayers = 2
        };
        print("正在加入房間或創建房間");
        PhotonNetwork.JoinOrCreateRoom("test_room", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        print($"已經加入房間!! 目前有{PhotonNetwork.room.playerCount}人");
        CreatePlayerObject();
        if (PhotonNetwork.room.playerCount == 2)
        {
            ball.GetComponent<Rigidbody>().isKinematic = false;
            GameMaster._instance.StartTheGameCoroutine();
            
        }
    }

    public void CreatePlayerObject()
    {
        int id = PhotonNetwork.player.ID;
        GameObject startposition = startPositions[id - 1];
        GameObject player =  PhotonNetwork.Instantiate("PikachuF", startposition.transform.position, startposition.transform.rotation, 0);
        player.GetComponent<PlayMove>().Pos = startPositions[id - 1];
    }
      
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        print("有玩家加入");

    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        print("有玩家離線");
    }
}
