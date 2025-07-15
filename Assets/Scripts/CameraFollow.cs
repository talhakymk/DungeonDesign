using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Takip edilecek nesne
    public Vector3 offset = new Vector3(0, 2, -3); // Kameran�n konumu
    public float rotationSpeed = 5f;

    private float currentAngle = 0f;

    void LateUpdate()
    {
        // Mouse X hareketi al
        float horizontalInput = Input.GetAxis("Mouse X");
        currentAngle += horizontalInput * rotationSpeed;

        // D�n�� a��s�n� hesapla
        Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Kameray� yeni pozisyona yerle�tir
        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
