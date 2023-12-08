using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public static Walk_Mode status;

    public float moveSpeed = 0.5f;
    public float rotationSpeed = 1200f;
    public float jumpForce = 0.03f;

    public float maxStamina = 100f;
    private float currentStamina;
    public float staminaDecreasePerSecond = 10f;
    public float staminaRecoveryPerSecond = 5f;

    private Rigidbody rb;
    private bool isJumping = false;
    private bool spawned = false;

    void Start()
    {
        status = Walk_Mode.Holding_Position;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        currentStamina = maxStamina;
    }

    void Update()
    {
        Spawn();
        Move();
        Rotate();
        Jump();
    }

    void Spawn()
    {
        if (TilePlacer.status == MapStatus.Loaded && !spawned)
        {
            transform.position = GameObject.FindWithTag("PlayerSpawnPoint").transform.position;
            spawned = true;
        }
        if (TilePlacer.status == MapStatus.Loading) 
        {
            spawned = false;
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX != 0 || moveZ != 0)
        {
            if (status != Walk_Mode.Attack)
                status = Walk_Mode.Walking;

            float currentSpeed = moveSpeed;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (currentStamina > 0)
                {
                    currentSpeed = moveSpeed * 2;
                    ConsumeStamina();
                }
            }
            else
            {
                RecoverStamina();
            }

            Vector3 moveDirection = new Vector3(moveX, 0f, moveZ);
            transform.Translate(currentSpeed * Time.deltaTime * moveDirection, Space.Self);
        }
        else
        {
            if (status != Walk_Mode.Attack)
                status = Walk_Mode.Holding_Position;

            RecoverStamina();
            return;
        }
    }


    void ConsumeStamina()
    {
        currentStamina -= staminaDecreasePerSecond * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0); // Предотвращение отрицательных значений выносливости
    }

    void RecoverStamina()
    {
        currentStamina += staminaRecoveryPerSecond * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina); // Предотвращение превышения максимальной выносливости
    }


    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(mouseX * rotationSpeed * Time.deltaTime * Vector3.up);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("NPC"))
        {
            isJumping = false;
        }
    }

    public void ChangeMoveSpeed(float value, float duration) 
    {
        StartCoroutine(MoveSpeedCoroutine(value, duration));
    }

    private IEnumerator MoveSpeedCoroutine(float value_, float duration_) {
        moveSpeed += value_;
        yield return new WaitForSeconds(duration_);
        moveSpeed -= value_;
    }
}
