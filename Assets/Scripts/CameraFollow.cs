using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Takip edilecek nesne
    public Vector3 offset = new Vector3(0, 2, -3); // Kameran�n konumu
    public float rotationSpeed = 5f;

    private float currentXAngle = 0f;
    private float currentYAngle = 0f;


    void LateUpdate()
    {
        // Mouse X hareketi al
        float horizontalXInput = Input.GetAxis("Mouse X");
        currentXAngle += horizontalXInput * rotationSpeed;

        float horizontalYInput = Input.GetAxis("Mouse Y");
        currentYAngle += horizontalYInput * rotationSpeed;


        // D�n�� a��s�n� hesapla
        Quaternion rotation = Quaternion.Euler(0f, currentXAngle, currentYAngle);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Kameray� yeni pozisyona yerle�tir
        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
