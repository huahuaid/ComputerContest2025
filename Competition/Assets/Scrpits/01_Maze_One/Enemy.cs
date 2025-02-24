using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public GameObject hurt;

    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float detectionRange = 0.5f;
    public Transform player;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 targetDirection;
    private bool isMovingTowardsPlayer = false;
    private bool isFacingRight = true; // 默认朝右

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializePatrol();
    }

    void Update()
    {
        if (!isMovingTowardsPlayer)
        {
            PatrolMovement();
        }
        else
        {
            MoveTowardsPlayer();
        }
        ChasePlayer();
        HandleRotation();
    }

    void InitializePatrol()
    {
        int randomDir = Random.Range(0, 4);
        moveDirection = GetDirectionFromAngle(randomDir * 90f);
        UpdateFacingDirection(moveDirection); // 初始化朝向
    }

    void PatrolMovement()
    {
        if (CanMove(moveDirection))
        {
            Vector3 movement = moveDirection * patrolSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            UpdateFacingDirection(moveDirection); // 更新朝向
        }
        else
        {
            FindNewPath();
        }
    }

    bool CanMove(Vector3 direction)
    {
        return !Physics.Raycast(transform.position, direction, detectionRange);
    }

    void FindNewPath()
    {
        for (int i = 0; i < 4; i++)
        {
            float randomAngle = Random.Range(0, 4) * 90;
            Vector3 newDirection = GetDirectionFromAngle(randomAngle);
            if (CanMove(newDirection))
            {
                moveDirection = newDirection;
                UpdateFacingDirection(moveDirection); // 更新朝向
                return;
            }
        }

        moveDirection *= -1;
        UpdateFacingDirection(moveDirection); // 更新朝向
    }

    void HandleRotation()
    {
        if (player != null)
        {
            gameObject.transform.rotation = player.transform.rotation;
        }
    }

    Vector3 GetDirectionFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)).normalized;
    }

    void ChasePlayer()
    {
        RaycastHit hit;
        float chaseDetectionRange = 10f; // 追逐检测范围
        Vector3[] directions = {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };

        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(transform.position, direction, out hit, chaseDetectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    targetDirection = direction;
                    isMovingTowardsPlayer = true;
                    UpdateFacingDirection(targetDirection); // 更新朝向
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (CanMove(targetDirection))
        {
            Vector3 movement = targetDirection * chaseSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            UpdateFacingDirection(targetDirection); // 更新朝向
        }
        else
        {
            isMovingTowardsPlayer = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("attack", true);
            animator.SetBool("walk", false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("attack", false);
            animator.SetBool("walk", true);
        }
    }

    void ActivateHurtArea()
    {
        hurt.SetActive(true);
        StartCoroutine(DeactivateAfterDelay(0.5f));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hurt.SetActive(false);
    }

    // 更新精灵的朝向
    void UpdateFacingDirection(Vector3 direction)
    {
        if (direction.x > 0 && !isFacingRight) // 向右移动
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight) // 向左移动
        {
            Flip();
        }
    }

    // 翻转精灵
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // 反转 X 轴
        transform.localScale = scale;
    }
}
