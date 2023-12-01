using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    [HideInInspector] public static GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
