using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject loadingMenu;
    public GameObject roomMenu;
    public GameObject mainMenu;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingMenu.SetActive(false);
        roomMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void CreateOrJoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        loadingMenu.SetActive(true);
        roomMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No random room available, creating a new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room. Current player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        loadingMenu.SetActive(false);
        roomMenu.SetActive(true);
        mainMenu.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public void LeaveRoom()
    {
        loadingMenu.SetActive(true);
        roomMenu.SetActive(false);
        mainMenu.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        loadingMenu.SetActive(false);
        roomMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
