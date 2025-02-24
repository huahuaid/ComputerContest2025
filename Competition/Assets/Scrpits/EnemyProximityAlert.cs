using UnityEngine;

public class EnemyProximityAlert : MonoBehaviour
{
	public Light playerLight;

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
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag("Enemy"))
		{
			if (ColorUtility.TryParseHtmlString("#FFE29B", out Color newColor))
			{
				playerLight.color = newColor;
			}
		}
	}
}
