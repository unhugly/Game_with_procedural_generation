using System.Collections;
using UnityEngine;

public class RespawningObject : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float respawnTime;

    private void Start()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);
        Respawn();
        StartCoroutine(RespawnRoutine());
    }

    private void Respawn()
    {
        if (GameObject.Find(prefab.name + "(Clone)") == null)
        {
            Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
