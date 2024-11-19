using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public PhotonView pv;
    public GameObject myCanvas;
    public GameObject allCanvas;

    public PlayerTurnManager player;

    public GameObject attackPalette;

    [Header("Prices")]
    public int movementPrice = 2;
    public int attackPrice = 2;

    [Header("Life")]
    public int life = 15;
    public Text lifeText;

    [Header("Shield")]
    public int shield = 5;
    public Text shieldText;


    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && pv.IsMine)
        {
            if (player.canPlay)
            {
                myCanvas.SetActive(!myCanvas.activeSelf);
            }

            allCanvas.SetActive(!allCanvas.activeSelf);

            if (!myCanvas.activeSelf)
            {
                attackPalette.SetActive(false);
            }
        }


        if (!pv.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                allCanvas.SetActive(!allCanvas.activeSelf);
            }
        }
    }

    public void Move(string dir)
    {
        if (player.movement >= movementPrice)
        {
            attackPalette.SetActive(false);
            player.movement -= movementPrice;

            RaycastHit hit;
            Vector3 newPos = transform.position;
            Vector3 direction = Vector3.zero;

            if (dir == "right")
            {
                newPos += Vector3.forward;
                direction = Vector3.forward; // Direction to look
            }
            else if (dir == "down")
            {
                newPos += Vector3.right;
                direction = Vector3.right; // Direction to look
            }
            else if (dir == "left")
            {
                newPos += Vector3.back;
                direction = Vector3.back; // Direction to look
            }
            else if (dir == "up")
            {
                newPos += Vector3.left;
                direction = Vector3.left; // Direction to look
            }

            // Check if there is a valid hit below the new position
            if (Physics.Raycast(newPos, Vector3.down, out hit, 5))
            {
                if (hit.transform.CompareTag("Dice_Blue") || hit.transform.CompareTag("Dice_Red"))
                {
                    // Move the object
                    transform.position = newPos;

                    // Rotate to look in the direction of movement
                    transform.rotation = Quaternion.LookRotation(direction);

                    Debug.Log($"Monster moved {dir} by 1 unit");
                }
            }
        }
    }


    public void Attack()
    {
        if (pv.IsMine)
        {
            attackPalette.SetActive(!attackPalette.activeSelf);
        }
    }


    private void Update()
    {
        if (pv.IsMine)
        {
            pv.RPC("LifeText_RPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void LifeText_RPC()
    {
        lifeText.text = "HP / " + life;
        shieldText.text = "DEF / " + shield;
    }

    public void TakeDamage(int damage)
    {
        pv.RPC("TakeDamage_RPC", RpcTarget.All, damage);
    }

    [PunRPC]
    public void TakeDamage_RPC(int damage)
    {
        int newDmg = damage -= shield;
        if(newDmg < 0)
        {
            newDmg = 0;
        }
        life -= newDmg;
        if(life <= 0)
        {
            if (pv.IsMine)
            {
                transform.position = new Vector3(100, -100, 100);
                foreach(Canvas go in GetComponentsInChildren<Canvas>())
                {
                    go.gameObject.SetActive(false);
                }
            }
        }
    }
}
