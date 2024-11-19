using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public GameObject NetworkMonster;

    public bool canPlace = false;

    public bool TeamBlue = true;


    public void OnTriggerStay(Collider other)
    {
        if (TeamBlue)
        {
            if (other.tag == "Dice_Blue")
            {
                canPlace = true;
            }
        }
        else
        {
            if (other.tag == "Dice_Red")
            {
                canPlace = true;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (TeamBlue)
        {
            if (other.tag == "Dice_Blue")
            {
                canPlace = false;
            }
        }
        else
        {
            if (other.tag == "Dice_Red")
            {
                canPlace = false;
            }
        }
    }
}
