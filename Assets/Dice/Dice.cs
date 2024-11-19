using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;

        ApplyRandomSpin();
    }

    private void ApplyRandomSpin()
    {
        float torqueX = Random.Range(-10f, 10f);
        float torqueY = Random.Range(-10f, 10f);
        float torqueZ = Random.Range(-10f, 10f);

        rb.AddTorque(new Vector3(torqueX, torqueY, torqueZ), ForceMode.Impulse);
    }
}
