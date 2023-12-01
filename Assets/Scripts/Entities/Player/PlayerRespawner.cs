using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;

    void Update()
    {
        if (transform.position.y < -5)
        {
            transform.position = spawnPoint.transform.position;
        }
    }
}
