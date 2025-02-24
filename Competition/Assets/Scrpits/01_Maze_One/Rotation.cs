using UnityEngine;

public class Rotation : MonoBehaviour
{
	public GameObject player;
	void Start()
	{

	}

	void Update()
	{
		gameObject.transform.rotation = player.transform.rotation;
	}
}
