using UnityEngine;

public class CameraObstacleDetection : MonoBehaviour
{
    public Transform player;
    public string playerTag = "Player";
    public string wallTag = "Wall";
    public LayerMask obstacleLayer;
    public Material transparentMaterial;

	public Material originalWallMaterial;
	private GameObject lastWall;

	private void Update()
	{
		// 检查从摄像机到 Player 之间是否有墙体
		if (CheckForWall(out GameObject wall))
		{
			// 如果检测到的 Wall 和上一次不同，恢复上一次 Wall 的材质
			if (lastWall != null && lastWall != wall)
			{
				ResetWallMaterial();
			}

			// 设置当前 Wall 的透明材质
			SetWallMaterial(wall);
			lastWall = wall;
		}
		else
		{
			// 如果没有检测到 Wall，恢复上一次 Wall 的材质
			if (lastWall != null)
			{
				ResetWallMaterial();
				lastWall = null;
			}
		}
	}

	bool CheckForWall(out GameObject wall)
	{
		wall = null;

		// 计算从摄像机到 Player 的方向和距离
		Vector3 direction = player.position - transform.position;
		float distance = direction.magnitude;

		// 发射射线
		RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, obstacleLayer);

		// 遍历所有击中的物体
		foreach (var hit in hits)
		{
			// 如果击中的物体不是 Player
			if (!hit.collider.CompareTag(playerTag))
			{
				// 如果击中的物体是 Wall
				if (hit.collider.CompareTag(wallTag))
				{
					wall = hit.collider.gameObject;
					return true;
				}
			}
		}

		// 如果没有检测到 Wall，返回 false
		return false;
	}

	void SetWallMaterial(GameObject wall)
	{
		Renderer renderer = wall.GetComponent<Renderer>();
		if (renderer != null)
		{
			renderer.material = transparentMaterial;
		}
	}

	void ResetWallMaterial()
	{
		if (lastWall != null && originalWallMaterial != null)
		{
			Renderer renderer = lastWall.GetComponent<Renderer>();
			if (renderer != null)
			{
				renderer.material = originalWallMaterial;
			}
		}
	}

	// 在 Scene 视图中绘制射线（用于调试）
	private void OnDrawGizmos()
	{
		if (player != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, player.position);
		}
	}
}
