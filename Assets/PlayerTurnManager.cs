using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerTurnManager : MonoBehaviour
{
    public PhotonView pv;


    public GameObject turnCanvas;

    [Header("Monsters")]
    public GameObject monsterCanvas;
    public GameObject monsterCanvas2;


    public GameObject mainCanvas;
    public GameObject _camera;



    [HideInInspector]
    public bool canEndTurn = true;
    public int returnedValues = 0;
    bool hasGotValues = false;
    public List<GameObject> dicesList = new List<GameObject>();

    [Header("Main Variables")]
    public bool canPlay = true;
    public bool hasRolled = false;

    [Header("Dice Manager")]
    public GameObject dice;
    public GameObject dice2;
    public GameObject[] spawners;

    [Header("Variables")]
    public int movement;
    public int attack;
    public int Level_1;
    public int Level_2;

    [Header("Texts")]
    public Text movementText;
    public Text attackText;


    [Header("Player Life")]
    public int life = 3;
    public GameObject[] lifeObj;


    public void TakeDamage()
    {
        if (pv.IsMine)
        {
            pv.RPC("TakeDamage_RPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void TakeDamage_RPC()
    {
        lifeObj[life - 1].SetActive(false);
        life--;
    }


    public void Start()
    {
        if (pv.IsMine)
        {

        }
        else
        {
            _camera.SetActive(false);
            mainCanvas.SetActive(false);
        }
    }

    public void StartTurn()
    {
        if (pv.IsMine)
        {
            turnCanvas.SetActive(true);
            canPlay = true;
            hasRolled = false;
        }
    }


    public void RollDice_1()
    {
        if (!hasRolled && pv.IsMine)
        {
            canPlay = false;
            hasRolled = true;

            foreach (GameObject GO in spawners)
            {
                GameObject _dice = PhotonNetwork.Instantiate(dice.name, GO.transform.position, GO.transform.rotation);
                dicesList.Add(_dice);
                _dice.GetComponent<DiceController>().PTM = this;
            }
        }
    }

    public void RollDice_2()
    {
        if (!hasRolled && pv.IsMine)
        {
            canPlay = false;
            hasRolled = true;

            foreach (GameObject GO in spawners)
            {
                GameObject _dice = PhotonNetwork.Instantiate(dice2.name, GO.transform.position, GO.transform.rotation);
                dicesList.Add(_dice);
                _dice.GetComponent<DiceController>().PTM = this;
            }
        }
    }


    public void Update()
    {
        if (pv.IsMine)
        {
            if (!hasGotValues)
            {
                if (returnedValues == 3)
                {
                    if (Level_1 == 3)
                    {
                        monsterCanvas.SetActive(true);
                    }
                    if (Level_2 == 3)
                    {
                        monsterCanvas2.SetActive(true);
                    }
                    hasGotValues = true;
                    canPlay = true;
                }
            }

            SetText();
        }
    }

    public void SetText()
    {
        if (pv.IsMine)
        {
            movementText.text = movement + "";
            attackText.text = attack + "";
        }
    }

    public void EndTurn()
    {
        if (hasGotValues && pv.IsMine && canEndTurn)
        {
            foreach (GameObject GO in dicesList)
            {
                PhotonNetwork.Destroy(GO);
            }
            dicesList.Clear();


            Level_1 = 0;
            Level_2 = 0;


            returnedValues = 0;
            hasRolled = false;
            canPlay = false;
            hasGotValues = false;

            monsterCanvas.SetActive(false);
            monsterCanvas2.SetActive(false);

            turnCanvas.SetActive(false);

            GameObject[] ends = GameObject.FindGameObjectsWithTag("EndTurnObject");

            foreach(GameObject GO in ends)
            {
                GO.SetActive(false);
            }

            GameManager.Instance.NextTurn();
        }
    }


}
