using UnityEngine;

public class Compass : MonoBehaviour
{
	public Transform player; 
	public Transform southTarget;
	public RectTransform compassImage;
	public RectTransform needle;

	private void Update()
	{
		Vector3 direction = southTarget.position - player.position;
		direction.y = 0;

		float angle = Vector3.SignedAngle(player.forward, direction, Vector3.up);

		compassImage.localEulerAngles = new Vector3(0, 0, -angle);
		needle.localEulerAngles = new Vector3(0, 0, -angle);
	}
}
