using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public GameObject[] playersArray;  // Array to store player objects
    public PhotonView pv;

    public int playerTurnIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (pv.IsMine)
        {
            StartCoroutine("TimerBeforeGameStarts");
        }
    }

    // Coroutine to wait before starting the game
    public IEnumerator TimerBeforeGameStarts()
    {
        yield return new WaitForSeconds(2f);  // Delay before starting the game
        StartGame();
        StartTurn();
    }

    // Start the game and set players
    public void StartGame()
    {
        pv.RPC("SetPlayers_RPC", RpcTarget.All);
    }

    // RPC to set up players and sort them by PhotonView ID
    [PunRPC]
    public void SetPlayers_RPC()
    {
        // Find players by tag (ensure your player objects have the "Player" tag in Unity)
        playersArray = GameObject.FindGameObjectsWithTag("Player");

        // Sort players by PhotonView ViewID to ensure consistent turn order
        System.Array.Sort(playersArray, (p1, p2) =>
        {
            PhotonView pv1 = p1.GetComponent<PhotonView>();
            PhotonView pv2 = p2.GetComponent<PhotonView>();
            return pv1.ViewID.CompareTo(pv2.ViewID);  // Compare ViewID for consistent ordering
        });
    }

    // Start the turn of the current player
    public void StartTurn()
    {
        pv.RPC("StartTurn_RPC", RpcTarget.All);
    }

    // RPC to start the turn for the current player
    [PunRPC]
    public void StartTurn_RPC()
    {
        if (playersArray != null && playersArray.Length > 0)
        {
            // Trigger the StartTurn method on the player's Controller
            playersArray[playerTurnIndex].GetComponent<PlayerTurnManager>().StartTurn();
        }
    }

    // Move to the next player's turn
    public void NextTurn()
    {
        pv.RPC("NextTurn_RPC", RpcTarget.All);
    }

    // RPC to handle the next player's turn
    [PunRPC]
    public void NextTurn_RPC()
    {
        playerTurnIndex++;
        if (playerTurnIndex == playersArray.Length)
        {
            playerTurnIndex = 0;  // Loop back to the first player
        }

        // Start the turn for the next player
        playersArray[playerTurnIndex].GetComponent<PlayerTurnManager>().StartTurn();
    }
}
