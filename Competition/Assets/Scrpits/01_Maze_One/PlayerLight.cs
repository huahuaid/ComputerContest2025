using UnityEngine;
using System.Collections.Generic;

public class PlayerLight : MonoBehaviour
{
    [Header("光源设置")]
    public float lightRadius = 5f;
    public LayerMask enemyLayer;

	private HashSet<EnemyVisibility> litEnemies = new HashSet<EnemyVisibility>();

	void Update()
	{
		HashSet<EnemyVisibility> currentFrameEnemies = new HashSet<EnemyVisibility>();

		Collider[] enemies = Physics.OverlapSphere(transform.position, lightRadius, enemyLayer);

		foreach (Collider enemyCol in enemies)
		{
			if (!HasObstacleBetween(enemyCol.transform))
			{
				EnemyVisibility enemy = enemyCol.GetComponent<EnemyVisibility>();
				if (enemy != null)
				{
					enemy.SetVisible(true);
					currentFrameEnemies.Add(enemy);
				}
			}
		}

		foreach (EnemyVisibility enemy in litEnemies)
		{
			if (!currentFrameEnemies.Contains(enemy) && enemy != null)
			{
				enemy.SetVisible(false);
			}
		}

		litEnemies = currentFrameEnemies;
	}

	bool HasObstacleBetween(Transform target)
	{
		Vector3 direction = target.position - transform.position;
		RaycastHit hit;

		if (Physics.Raycast(transform.position, direction, out hit, lightRadius))
		{
			return hit.transform != target;
		}
		return false;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, lightRadius);
	}
}
