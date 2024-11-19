using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;
    private bool resultChecked = false;

    public List<FaceWithPlanes> facesList;

    public PlayerTurnManager PTM;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Roll();
    }

    public void Roll()
    {
        isRolling = true;
        resultChecked = false;

        rb.AddForce(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        StartCoroutine(CheckIfDiceStopped());
    }

    private IEnumerator CheckIfDiceStopped()
    {
        yield return new WaitForSeconds(0.2f);
        while (rb.velocity.magnitude > 0f && rb.angularVelocity.magnitude > 0f)
        {
            yield return null;
        }

        if (!resultChecked)
        {
            resultChecked = true;
            DetectFaceType();
        }
    }

    public void DetectFaceType()
    {
        PTM.returnedValues++;

        if(CheckDiceFace().faceType == FaceType.MOVEMENT)
        {
            PTM.movement++;
        }
        if (CheckDiceFace().faceType == FaceType.ATTACK)
        {
            PTM.attack++;
        }
        if (CheckDiceFace().faceType == FaceType.LEVEL_1)
        {
            PTM.Level_1++;
        }
        if (CheckDiceFace().faceType == FaceType.LEVEL_2)
        {
            PTM.Level_2++;
        }
    }

    private FaceWithPlanes CheckDiceFace()
    {
        Vector3 dicePos = (transform.position + Vector3.up * 2f);

        RaycastHit hit;
        if(Physics.Raycast(dicePos, Vector3.down, out hit, 10f))
        {
            foreach(FaceWithPlanes FWB in facesList)
            {
                if (hit.collider.gameObject == FWB.plane)
                {
                    return FWB;
                }
            }
        }
        return null;
    }
}


[System.Serializable]
public enum FaceType
{
    MOVEMENT,
    ATTACK,
    LEVEL_1,
    LEVEL_2,
    LEVEL_3,
    LEVEL4
}


[System.Serializable]
public class FaceWithPlanes
{
    public FaceType faceType;
    public GameObject plane;
}
