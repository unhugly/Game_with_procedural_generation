using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RandomWalker : MonoBehaviour
{
    [HideInInspector] public Walk_Mode walk_Mode;

    public float maxDistanceFromSpawn = 1.0f;
    public float detectionRadius = 1.0f;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 10.0f;
    public float standUpSpeed = 1.0f;
    public float timeout = 5.0f;  // Таймаут для достижения цели
    private float timeSinceLastTargetSet;  // Время, прошедшее с момента установки последней цели
    private float timeSinceLastMovement;

    private Vector3 spawnPoint;
    private Vector3 targetPosition;
    private Vector3 previousPosition;

    private void Start()
    {
        spawnPoint = transform.position;
        walk_Mode = Walk_Mode.Walking;
    }

    private void Update()
    {
        switch (walk_Mode) 
        {
            case Walk_Mode.Walking:
                Walk();
                break;
            case Walk_Mode.Holding_Position:
                HoldPosition();
                break;
            case Walk_Mode.Attack:
                break;
            default:
                break;
        }
    }

    void FixZXRotation()
    {
        float yRotation = transform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, yRotation, 0);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, standUpSpeed * Time.fixedDeltaTime));
    }

    private void Walk() 
    {
        if (IsPlayerInRange())
        {
            walk_Mode = Walk_Mode.Holding_Position;
            return;
        }
        if ((transform.position - targetPosition).magnitude > 0.1f)
        {
            if (IsPathBlocked() || timeSinceLastTargetSet > timeout)
            {
                SetRandomTargetPosition();
                timeSinceLastTargetSet = 0f;
            }
            else
            {
                MoveTowardsTarget();
                RotateTowardsDirection();
            }
        }
        timeSinceLastTargetSet += Time.deltaTime;
        CheckForStagnation();
    }

    private bool IsPlayerInRange()
    {
        if (PlayerReference.player == null)
        {
            Debug.LogWarning("Player object with tag 'Player' not found!");
            return false;
        }
        return Vector3.Distance(transform.position, PlayerReference.player.transform.position) < detectionRadius;
    }

    void HoldPosition()
    {
        Vector3 direction = (PlayerReference.player.transform.position - transform.position);
        if (direction.magnitude > Mathf.Epsilon)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        if (!IsPlayerInRange()) 
        {
            walk_Mode = Walk_Mode.Walking;
        }
    }

    private void CheckForStagnation()
    {
        if (Vector3.Distance(transform.position, previousPosition) < 0.01f)
        {
            timeSinceLastMovement += Time.deltaTime;
            if (timeSinceLastMovement > timeout)
            {
                SetRandomTargetPosition();
                timeSinceLastMovement = 0f;
            }
        }
        else
        {
            timeSinceLastMovement = 0f;
        }

        previousPosition = transform.position;
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if ((transform.position - targetPosition).magnitude < 0.1f)
        {
            StartCoroutine(WaitAtTarget());
        }
    }

    private void RotateTowardsDirection()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SetRandomTargetPosition()
    {
        Vector3 randomDirection;
        int maxAttempts = 5;  // Максимальное количество попыток найти достижимую цель
        int currentAttempt = 0;

        do
        {
            randomDirection = Random.insideUnitSphere * maxDistanceFromSpawn;
            randomDirection.y = 0;
            targetPosition = spawnPoint + randomDirection;
            currentAttempt++;
        } while (IsPathBlocked() && currentAttempt < maxAttempts);
    }

    private IEnumerator WaitAtTarget()
    {
        float randomWaitTime = Random.Range(2.0f, 5.0f);
        yield return new WaitForSeconds(randomWaitTime);
        SetRandomTargetPosition();
        timeSinceLastTargetSet = 0f;
    }

    private bool IsPathBlocked()
    {
        RaycastHit hit;
        float distanceToTarget = (targetPosition - transform.position).magnitude;
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        if (Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }
}
