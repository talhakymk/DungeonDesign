using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GandalfSpawner : MonoBehaviour
{
    public GameObject characterPrefab; // spawnlanacak karakter
    public Vector3 spawnOffset = new Vector3(1, 0, 0);
    public float checkRadius = 0.5f; // Spawn alaný kontrol yarýçapý
    public LayerMask obstacleLayerMask = 1; // Sadece Default layer'ý kontrol et
    public bool enableDebugGizmos = true; // Debug çizimlerini aktif et

    private GameObject spawnedCharacter; // Tek karakter için
    private GameObject currentObject; // Hangi obje ile temas halinde olduðumuzu takip et

    void Update()
    {
        // R tuþuna basýldýðýnda karakteri yok et
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
        // Eðer "Object" tag'li obje ile temas ederse
        if (other.CompareTag("Object"))
        {
            // Eðer zaten bir karakter spawn edilmiþse, yeni spawn etme
            if (spawnedCharacter == null)
            {
                Vector3 safeSpawnPosition = FindSafeSpawnPosition(other.transform.position);

                if (safeSpawnPosition != Vector3.zero)
                {
                    // Güvenli pozisyonda karakter spawn et
                    spawnedCharacter = Instantiate(characterPrefab, safeSpawnPosition, Quaternion.identity);
                    currentObject = other.gameObject;
                }
                else
                {
                    // Güvenli pozisyon bulunamadýðýnda basit offset kullan
                    Debug.LogWarning("Güvenli pozisyon bulunamadý, basit offset kullanýlýyor!");
                    spawnedCharacter = Instantiate(characterPrefab, other.transform.position + spawnOffset, Quaternion.identity);
                    currentObject = other.gameObject;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Bu metod artýk karakter silmiyor, sadece referansý temizliyor
        if (other.CompareTag("Object") && other.gameObject == currentObject)
        {
            currentObject = null;
        }
    }

    Vector3 FindSafeSpawnPosition(Vector3 originPosition)
    {
        // Farklý yönlerde spawn pozisyonlarý dene
        Vector3[] offsets = {
            new Vector3(1, 0, 0),   // Sað
            new Vector3(-1, 0, 0),  // Sol
            new Vector3(0, 0, 1),   // Ýleri
            new Vector3(0, 0, -1),  // Geri
            new Vector3(1, 0, 1),   // Sað-Ýleri
            new Vector3(-1, 0, 1),  // Sol-Ýleri
            new Vector3(1, 0, -1),  // Sað-Geri
            new Vector3(-1, 0, -1)  // Sol-Geri
        };

        foreach (Vector3 offset in offsets)
        {
            Vector3 testPosition = originPosition + offset * spawnOffset.magnitude;

            Debug.Log($"Test pozisyonu: {testPosition}, CheckSphere sonucu: {Physics.CheckSphere(testPosition, checkRadius, obstacleLayerMask)}");

            // Pozisyonda engel var mý kontrol et
            if (!Physics.CheckSphere(testPosition, checkRadius, obstacleLayerMask))
            {
                // Zeminde mi kontrol et (raycast ile)
                RaycastHit hit;
                if (Physics.Raycast(testPosition + Vector3.up * 5f, Vector3.down, out hit, 10f))
                {
                    Vector3 finalPosition = hit.point + Vector3.up * 0.1f;
                    Debug.Log($"Güvenli pozisyon bulundu: {finalPosition}");
                    return finalPosition; // Zeminden biraz yukarýda
                }
                else
                {
                    Debug.Log($"Zeminde deðil: {testPosition}");
                }
            }
        }

        // Güvenli pozisyon bulunamadý
        Debug.LogWarning("Güvenli spawn pozisyonu bulunamadý!");
        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        if (enableDebugGizmos && currentObject != null)
        {
            // Test pozisyonlarýný görselleþtir
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
