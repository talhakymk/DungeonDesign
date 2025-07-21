using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GandalfSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject characterPrefab; // spawnlanacak karakter
    public float spawnDistance = 1f;

    private GameObject spawnedCharacter;

    void Update()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(player.transform.position, obj.transform.position);

            if (distance <= spawnDistance)
            {
                if (spawnedCharacter == null)
                {
                    // Instantiate karakter
                    spawnedCharacter = Instantiate(characterPrefab, obj.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                }
            }
           
        }
    }
}
