using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // This will no longer handle adding players to the list. It's handled in GameManager.
            // Just instantiate the player prefab here
            Quaternion spawnRotation = Quaternion.identity;

            if (PhotonNetwork.LocalPlayer.ActorNumber == 1) // First player
            {
                spawnRotation = Quaternion.Euler(0, 0, 0); // y = 0
                GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, spawnRotation);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2) // Second player
            {
                spawnRotation = Quaternion.Euler(0, 0, 0); // y = 180
                GameObject player = PhotonNetwork.Instantiate("Player2", Vector3.zero, spawnRotation);
            }

        }
    }
}
