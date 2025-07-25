using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Takip edilecek nesne
    public Vector3 offset = new Vector3(0, 2, -3); // Kameran�n konumu

    void LateUpdate()
    {
        // Kameray� karakterin arkas�nda tutmak i�in offset'i karakterin rotasyonuna g�re uygula
        Vector3 desiredPosition = target.position + target.rotation * offset;
        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}