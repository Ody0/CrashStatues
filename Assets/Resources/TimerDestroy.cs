using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TimerDestroy : MonoBehaviour
{


    public int damage = 10;


    public void Start()
    {
        StartCoroutine("DestroyObj");
    }

    public IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(1.2f);
        GetComponent<PhotonView>().RPC("Destroy_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void Destroy_RPC()
    {
        PhotonNetwork.Destroy(gameObject);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (other.GetComponent<Monster>())
            {
                other.GetComponent<Monster>().TakeDamage(damage);
            }
        }
    }
}
