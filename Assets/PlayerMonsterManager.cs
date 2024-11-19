using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerMonsterManager : MonoBehaviour
{
    public PhotonView pv;

    public LayerMask pathLayer;


    public GameObject actualObj;

    public Transform pos;

    public Camera cam;

    public GameObject monsterCanvas;
    public GameObject mainCanvas;

    public bool BlueTeam = false;

    public void Blue_Spawn(GameObject Blueprint)
    {
        if (pv.IsMine)
        {
            mainCanvas.SetActive(false);
            monsterCanvas.SetActive(false);
            actualObj = Instantiate(Blueprint, Vector3.zero, pos.rotation);
        }
    }

    public void Red_Spawn(GameObject Blueprint)
    {
        if (pv.IsMine)
        {
            mainCanvas.SetActive(false);
            monsterCanvas.SetActive(false);
            actualObj = Instantiate(Blueprint, Vector3.zero, pos.rotation);
        }
    }

    int rot = 0;

    void Update()
    {
        if (pv.IsMine)
        {
            if (actualObj != null)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, pathLayer))
                {
                    pos.position = hit.point;
                }

                actualObj.transform.position = new Vector3(Mathf.Round(pos.transform.position.x), Mathf.Round(pos.transform.position.y), Mathf.Round(pos.transform.position.z));
                actualObj.transform.rotation = Quaternion.Euler(0f, rot, 0f);
            }

            if(Input.GetMouseButtonDown(0) && actualObj != null && actualObj.GetComponent<Blueprint>().canPlace)
            {
                if (BlueTeam)
                {
                    GameObject monster = PhotonNetwork.Instantiate(Path.Combine("Blue", actualObj.GetComponent<Blueprint>().NetworkMonster.name), actualObj.transform.position, actualObj.transform.rotation);
                    monster.GetComponentInChildren<Monster>().player = GetComponent<PlayerTurnManager>();
                    GameObject _monst = monster.GetComponentInChildren<Monster>().gameObject;
                    pv.RPC("RemoveParent_RPC", RpcTarget.All, _monst.GetComponent<PhotonView>().ViewID);
                }
                else
                {
                    GameObject monster = PhotonNetwork.Instantiate(Path.Combine("Red", actualObj.GetComponent<Blueprint>().NetworkMonster.name), actualObj.transform.position, actualObj.transform.rotation);
                    monster.GetComponentInChildren<Monster>().player = GetComponent<PlayerTurnManager>();
                    GameObject _monst = monster.GetComponentInChildren<Monster>().gameObject;
                    pv.RPC("RemoveParent_RPC", RpcTarget.All, _monst.GetComponent<PhotonView>().ViewID);
                }
                Destroy(actualObj);
                actualObj = null;
                mainCanvas.SetActive(true);
            }

            if(Input.GetKeyDown(KeyCode.R) && actualObj != null)
            {
                rot += 90;
            }
        }
    }

    [PunRPC]
    public void RemoveParent_RPC(int id)
    {
        PhotonView.Find(id).gameObject.transform.SetParent(null);
    }

}
