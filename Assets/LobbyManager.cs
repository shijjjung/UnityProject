using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LobbyManager : Photon.PunBehaviour
{
    public GameObject[] startPositions;
    public const string VersionMark = "WOTv1.0";
    [SerializeField]
    Button joinButton, joinRandomButton, createButton;
    [SerializeField]
    ScrollRect scrollView;//序列化物件 需要使用他更新列表 另外需要RoomInfo陣列 儲存房間清單

    Transform template;

    RoomInfo[] roomList;
    [SerializeField]
    GameObject checkPassView;

    [SerializeField]
    Ball ball;
    Button selectedButton;
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(VersionMark);
        template = scrollView.content.GetChild(0);
        template.gameObject.SetActive(false);
    }
    void RefreshList()
    {
        //0跳過 不刪除
        //for (int i = 1; i < scrollView.content.childCount; i++)
        //{
        //    Destroy(scrollView.content.gameObject);
        //}
        foreach (Transform child in scrollView.content)
        {
            if (child != template) Destroy(child.gameObject);
        }
        template.gameObject.SetActive(true);
        Text[] labels = template.GetComponentsInChildren<Text>();
        roomList = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in roomList)
        {
            labels[0].text = room.Name;
            labels[1].text = room.PlayerCount + "/" + room.MaxPlayers;
            labels[2].text = ((string)room.CustomProperties["password"] == "") ? "否" : "是";
            Instantiate(template, scrollView.content);
        }
        template.gameObject.SetActive(false);
    }
    public override void OnReceivedRoomListUpdate()
    {
        //房間狀態發生變化時更新
        RefreshList();
    }
    public override void OnConnectedToMaster()
    {
        print("與Master Sercver 建立連線");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print("已進入大廳");
        joinButton.interactable = true;
        joinRandomButton.interactable = true;
        createButton.interactable = true;
    }

    public void CreateRoom(string roomName, RoomOptions options)
    {
        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
    }
    // Update is called once per frame
    void Update()
    {

    }

    //防止房間名稱重複 一般使用Callback
    public override void OnCreatedRoom()
    {
        print("已加入房間");
        print(PhotonNetwork.lobby + "Room: " + PhotonNetwork.room.ToStringFull());
        //轉入房間場景
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
        print(PhotonNetwork.room.ToStringFull());
    }


    public void CreatePlayerObject()
    {
        int id = PhotonNetwork.player.ID;
        GameObject startposition = startPositions[id - 1];
        PhotonNetwork.Instantiate("PikachuF", startposition.transform.position, startposition.transform.rotation, 0);
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        print($"{codeAndMsg[0]} : {codeAndMsg[1]}"); ;
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        print($"{codeAndMsg[0]} : {codeAndMsg[1]}"); ;
    }
    public void OnSearchValueChange(string value)
    {
        for (int i = 0; i < roomList.Length; i++)
        {
            //if (roomList[i].Name.IndexOf(value) > -1)
            bool active = roomList[i].Name.Contains(value);
            scrollView.content.GetChild(i + 1).gameObject.SetActive(active);
        }
    }
    public void OnRoomSelected(Button roomButton)
    {
        roomButton.interactable = false;
        if (selectedButton != null)
        {
            selectedButton.interactable = true;
        }
        selectedButton = roomButton;
    }
    public void OnJoinRoomClick()
    {
        //Debug.Log(selectedButton);
        if (selectedButton == null) return;//未選擇房間
        int index = selectedButton.transform.GetSiblingIndex() - 1;//第幾個物件 扣掉template 序號
        RoomInfo roomInfo = roomList[index];
        print(roomInfo.ToStringFull());
        if ((string)roomInfo.CustomProperties["password"] == "")
        {
            PhotonNetwork.JoinRoom(roomList[index].Name);
            //Debug.Log("我沒有密碼");
        }
        else
        {
            //Debug.Log("" +
            //    "PASS");
            //密碼
            checkPassView.SetActive(true);
        }
    }
    public void OnCheckPasswordConfirm(string password)
    {
        int index = selectedButton.transform.GetSiblingIndex() - 1;
        RoomInfo roomInfo = roomList[index];
        if ((string)roomInfo.CustomProperties["password"] == password)
        {
            PhotonNetwork.JoinRoom(roomList[index].Name);
        }
    }

    public void OnJoinRandomClick()
    {
        var options = new ExitGames.Client.Photon.Hashtable
        {
            {"password","" } //密碼為空
        };
        PhotonNetwork.JoinRandomRoom(options, 0);//最少人數
    }

}
