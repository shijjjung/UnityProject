using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateView : MonoBehaviour {
    [SerializeField]
    InputField nameField, passwordField, infoField;
    [SerializeField]
    Dropdown playerCountSelector;
    [SerializeField]
    Toggle publicToggle;
    [SerializeField]
    LobbyManager lobbyManager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCreateClick()
    {
        string roomName = nameField.text;
        if (string.IsNullOrWhiteSpace(roomName)) return;
        int maxPlayers = playerCountSelector.value * 2 + 2;
        bool isVisible = publicToggle.isOn;

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayers,
            IsVisible = isVisible
        };
        //進入GameRoom 才能看到Properties
        var customProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "info",infoField.text},
            { "password",passwordField.text}
        };
        options.CustomRoomProperties = customProperties;
        //哪些項目是可以在大廳看到的 CustomRoomPropertiesForLobby
        options.CustomRoomPropertiesForLobby = new string[] { "password" };
        lobbyManager.CreateRoom(roomName, options);
    }
}
