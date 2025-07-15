using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveForce = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D veya sol/sa�
        float moveZ = Input.GetAxis("Vertical");   // W/S veya yukar�/a�a��

        Vector3 force = new Vector3(moveX, 0, moveZ) * moveForce;
        rb.AddForce(force);
    }
}
