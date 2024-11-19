using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBaseManager : MonoBehaviour
{
    public PhotonView pv;


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Damage" && pv.IsMine)
        {
            pv.GetComponent<PlayerTurnManager>().TakeDamage();
        }
    }
}
