using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Takip edilecek nesne
    public Vector3 offset = new Vector3(0, 2, -3); // Kameranýn konumu
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


        // Dönüþ açýsýný hesapla
        Quaternion rotation = Quaternion.Euler(0f, currentXAngle, currentYAngle);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Kamerayý yeni pozisyona yerleþtir
        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
