using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamageZone : MonoBehaviour
{


    public GameObject damageObj;

    public Monster monster;



    public void OnMouseDown()
    {
        if (monster.player.pv.IsMine)
        {
            if (monster.player.attack >= monster.attackPrice)
            {
                monster.player.attack -= monster.attackPrice;
                PhotonNetwork.Instantiate(damageObj.name, transform.position, transform.rotation);
                monster.attackPalette.SetActive(false);
            }
        }
    }
}
