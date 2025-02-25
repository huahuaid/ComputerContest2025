using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Rigidbody player;
	public Animator animator;

	public float detectionRange = 1.0f;
	public float moveSpeed = 0.5f;

	public bool canMoveForward = true;
	public bool canMoveBackward = true;
	public bool canMoveRight = true;
	public bool canMoveLeft = true;

	public static int health = 3;

	void Start()
	{
		player = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		// RandomizeInitialRotation();
	}

	void Update()
	{
		Survive();
		UpdateMovementStatus();
		HandleMovementInput();
		HandlePerspective();
	}

	void UpdateMovementStatus()
	{
		canMoveForward = CanMove(transform.forward);
		canMoveBackward = CanMove(-transform.forward);
		canMoveRight = CanMove(transform.right);
		canMoveLeft = CanMove(-transform.right);
	}

	bool CanMove(Vector3 direction)
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
		{
			if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Enemy"))
			{
				return false;
			}
		}
		return true;
	}

	void HandleMovementInput()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = Vector3.zero;

		float currentYRotation = transform.eulerAngles.y;

		if (moveVertical > 0 && canMoveForward)
		{
			movement += GetDirectionFromAngle(currentYRotation);
		}
		if (moveVertical < 0 && canMoveBackward)
		{
			movement += GetDirectionFromAngle(currentYRotation + 180);
		}
		if (moveHorizontal > 0 && canMoveRight)
		{
			movement += GetDirectionFromAngle(currentYRotation + 90);
			transform.localScale = new Vector3(1, 1, 1);
		}
		if (moveHorizontal < 0 && canMoveLeft)
		{
			movement += GetDirectionFromAngle(currentYRotation - 90);
			transform.localScale = new Vector3(-1, 1, 1);
		}

		if (movement.magnitude > 0)
		{
			animator.SetBool("run", true);
			movement = movement.normalized * moveSpeed * Time.deltaTime;
			player.MovePosition(player.position + movement);
		}
		else
		{
			animator.SetBool("run", false);
		}
	}

	void HandlePerspective()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			transform.Rotate(0, -90, 0);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			transform.Rotate(0, 90, 0);
		}
	}

	Vector3 GetDirectionFromAngle(float angle)
	{
		float angleInRadians = angle * Mathf.Deg2Rad;
		return new Vector3(Mathf.Sin(angleInRadians), 0, Mathf.Cos(angleInRadians)).normalized;
	}

	// void RandomizeInitialRotation()
	// {
	// 	float[] possibleRotations = { 0f, 90f, 180f, 270f };
	// 	float randomRotation = possibleRotations[Random.Range(0, possibleRotations.Length)];
	// 	transform.rotation = Quaternion.Euler(0, randomRotation, 0);
	// }

	void Survive(){
		if (health <= 0)
		{
			animator.SetBool("death",true);
			StartCoroutine(DestroyAfterDelay(0.4f)); 
		}
	}

	IEnumerator DestroyAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay); 
		Destroy(gameObject);
	}
}
