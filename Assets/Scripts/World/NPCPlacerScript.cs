/*using UnityEngine;

public class NPCPlacerScript : MonoBehaviour
{
    public GameObject objectPrefab;
    public string spawnPointTag = "";
    private bool placed = false;

    private void Update()
    {
        if (placed == false && TilePlacer.status == MapStatus.Loaded) 
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);

            foreach (GameObject spawnPoint in spawnPoints)
            {
                Instantiate(objectPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
            placed = true;
        }
        if (TilePlacer.status == MapStatus.Empty) {
            placed = false;
        }
    }
}*/
using UnityEngine;
using System.Collections.Generic;

public class NPCPlacerScript : MonoBehaviour
{
    public List<GameObject> livingEntityPrefabs;
    private Dictionary<GameObject, GameObject> spawnedEntities = new Dictionary<GameObject, GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        foreach (var entityPrefab in livingEntityPrefabs)
        {
            LivingEntity entityScript = entityPrefab.GetComponent<LivingEntity>();
            if (entityScript != null && other.CompareTag(entityScript.spawnTag))
            {
                if (!spawnedEntities.ContainsKey(other.gameObject))
                {
                    var spawnedEntity = Instantiate(entityPrefab, other.transform.position, other.transform.rotation);
                    spawnedEntities.Add(other.gameObject, spawnedEntity);
                }
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (spawnedEntities.ContainsKey(other.gameObject))
        {
            Destroy(spawnedEntities[other.gameObject]);
            spawnedEntities.Remove(other.gameObject);
        }
    }
}