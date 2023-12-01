using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotates : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1.0f;
    private float verticalMovementAmplitude = 0.06f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        float newY = Mathf.Sin(Time.time) * verticalMovementAmplitude;
        transform.position = new Vector3(initialPosition.x, initialPosition.y + newY, initialPosition.z);
    }
}
