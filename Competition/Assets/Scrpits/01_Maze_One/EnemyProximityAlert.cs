using UnityEngine;

public class EnemyProximityAlert : MonoBehaviour
{
	public Light playerLight;
	public Animator animator;

	void Start()
	{

	}

	void Update()
	{

	}


	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Enemy"))
		{
			if (ColorUtility.TryParseHtmlString("#FF1700", out Color newColor))
			{
				playerLight.color = newColor;
				animator.SetBool("shake",true);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag("Enemy"))
		{
			if (ColorUtility.TryParseHtmlString("#FFE29B", out Color newColor))
			{
				playerLight.color = newColor;
				animator.SetBool("shake",false);
			}
		}
	}
}
