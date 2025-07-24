using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GandalfSpawner : MonoBehaviour
{
    public GameObject characterPrefab; // spawnlanacak karakter
    public Vector3 spawnOffset = new Vector3(1, 0, 0);
    public float checkRadius = 0.5f; // Spawn alan� kontrol yar��ap�
    public LayerMask obstacleLayerMask = 1; // Sadece Default layer'� kontrol et
    public bool enableDebugGizmos = true; // Debug �izimlerini aktif et

    private GameObject spawnedCharacter; // Tek karakter i�in
    private GameObject currentObject; // Hangi obje ile temas halinde oldu�umuzu takip et

    void Update()
    {
        // R tu�una bas�ld���nda karakteri yok et
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (spawnedCharacter != null)
            {
                Destroy(spawnedCharacter);
                spawnedCharacter = null;
                currentObject = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // E�er "Object" tag'li obje ile temas ederse
        if (other.CompareTag("Object"))
        {
            // E�er zaten bir karakter spawn edilmi�se, yeni spawn etme
            if (spawnedCharacter == null)
            {
                Vector3 safeSpawnPosition = FindSafeSpawnPosition(other.transform.position);

                if (safeSpawnPosition != Vector3.zero)
                {
                    // G�venli pozisyonda karakter spawn et
                    spawnedCharacter = Instantiate(characterPrefab, safeSpawnPosition, Quaternion.identity);
                    currentObject = other.gameObject;
                }
                else
                {
                    // G�venli pozisyon bulunamad���nda basit offset kullan
                    Debug.LogWarning("G�venli pozisyon bulunamad�, basit offset kullan�l�yor!");
                    spawnedCharacter = Instantiate(characterPrefab, other.transform.position + spawnOffset, Quaternion.identity);
                    currentObject = other.gameObject;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Bu metod art�k karakter silmiyor, sadece referans� temizliyor
        if (other.CompareTag("Object") && other.gameObject == currentObject)
        {
            currentObject = null;
        }
    }

    Vector3 FindSafeSpawnPosition(Vector3 originPosition)
    {
        // Farkl� y�nlerde spawn pozisyonlar� dene
        Vector3[] offsets = {
            new Vector3(1, 0, 0),   // Sa�
            new Vector3(-1, 0, 0),  // Sol
            new Vector3(0, 0, 1),   // �leri
            new Vector3(0, 0, -1),  // Geri
            new Vector3(1, 0, 1),   // Sa�-�leri
            new Vector3(-1, 0, 1),  // Sol-�leri
            new Vector3(1, 0, -1),  // Sa�-Geri
            new Vector3(-1, 0, -1)  // Sol-Geri
        };

        foreach (Vector3 offset in offsets)
        {
            Vector3 testPosition = originPosition + offset * spawnOffset.magnitude;

            Debug.Log($"Test pozisyonu: {testPosition}, CheckSphere sonucu: {Physics.CheckSphere(testPosition, checkRadius, obstacleLayerMask)}");

            // Pozisyonda engel var m� kontrol et
            if (!Physics.CheckSphere(testPosition, checkRadius, obstacleLayerMask))
            {
                // Zeminde mi kontrol et (raycast ile)
                RaycastHit hit;
                if (Physics.Raycast(testPosition + Vector3.up * 5f, Vector3.down, out hit, 10f))
                {
                    Vector3 finalPosition = hit.point + Vector3.up * 0.1f;
                    Debug.Log($"G�venli pozisyon bulundu: {finalPosition}");
                    return finalPosition; // Zeminden biraz yukar�da
                }
                else
                {
                    Debug.Log($"Zeminde de�il: {testPosition}");
                }
            }
        }

        // G�venli pozisyon bulunamad�
        Debug.LogWarning("G�venli spawn pozisyonu bulunamad�!");
        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        if (enableDebugGizmos && currentObject != null)
        {
            // Test pozisyonlar�n� g�rselle�tir
            Vector3[] offsets = {
                new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1),
                new Vector3(1, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1)
            };

            foreach (Vector3 offset in offsets)
            {
                Vector3 testPosition = currentObject.transform.position + offset * spawnOffset.magnitude;
                Gizmos.color = Physics.CheckSphere(testPosition, checkRadius, obstacleLayerMask) ? Color.red : Color.green;
                Gizmos.DrawWireSphere(testPosition, checkRadius);
            }
        }
    }
}
