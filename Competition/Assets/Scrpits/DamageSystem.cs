using UnityEngine;

public class DamageSystem : MonoBehaviour
{
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player"))
		{
			PlayerController.health -= 1;
		}
	}
}
