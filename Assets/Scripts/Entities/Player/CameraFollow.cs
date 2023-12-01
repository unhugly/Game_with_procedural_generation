using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.025f; // Скорость интерполяции
    public Vector3 offset;
    public float downwardAngle = 15f;
    public float rotationSpeed = 5f;

    private Vector3 initialOffset;
    private bool isAltMode = false; // Флаг режима свободного вращения

    private void Start()
    {
        initialOffset = target.TransformDirection(offset);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            isAltMode = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))
        {
            isAltMode = false;
        }
    }

    void LateUpdate()
    {
        if (isAltMode)
        {
            FreeRotateCamera();
        }
        else
        {
            FollowTarget();
        }
    }

    void FollowTarget()
    {
        Vector3 desiredPosition = target.position + target.TransformDirection(initialOffset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
        transform.Rotate(Vector3.right, downwardAngle);
    }

    void FreeRotateCamera()
    {
        Vector3 currentRotation = PlayerReference.player.transform.eulerAngles;
        PlayerReference.player.transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z);
        
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.RotateAround(target.position, Vector3.up, mouseX);

        Vector3 angles = transform.eulerAngles;
        angles.x = Mathf.Clamp(angles.x + mouseY, 0, 90);
        transform.eulerAngles = angles;
    }
}
