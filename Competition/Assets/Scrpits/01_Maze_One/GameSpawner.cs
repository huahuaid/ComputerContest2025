using UnityEngine;
using System.Collections.Generic;

public class GameSpawner : MonoBehaviour
{
	[Header("生成设置")]
		public GameObject player;
	public GameObject enemyPrefab;
	public int maxEnemies = 10;
	public float playerSafeRadius = 10f;
	public int maxAttempts = 30;

	[Header("生成范围")]
	public float xMin = -190f;
	public float xMax = -90f;
	public float zMin = -6f;
	public float zMax = 93f;
	public float fixedY = 7f;

	private List<GameObject> enemies = new List<GameObject>();

	void Start()
	{
		SpawnPlayer();
		SpawnEnemies();
	}

	void SpawnPlayer()
	{
		Vector3 spawnPos = FindValidSpawnPosition(true);
		if(spawnPos != Vector3.zero)
		{
			player.transform.position = spawnPos;
		}
	}

	void SpawnEnemies()
	{
		for(int i = 0; i < maxEnemies; i++)
		{
			Vector3 spawnPos = FindValidSpawnPosition(false);
			if(spawnPos != Vector3.zero)
			{
				GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
				enemies.Add(enemy);
			}
		}
	}

	Vector3 FindValidSpawnPosition(bool isPlayer)
	{
		Vector3 spawnPos = Vector3.zero;
		bool isValid = false;
		int attempts = 0;

		do
		{
			spawnPos = new Vector3(
					Random.Range(xMin, xMax),
					fixedY,
					Random.Range(zMin, zMax)
					);

			isValid = IsPositionValid(spawnPos, isPlayer);
			attempts++;

		} while (!isValid && attempts < maxAttempts);

		return isValid ? spawnPos : Vector3.zero;
	}

	bool IsPositionValid(Vector3 pos, bool isPlayer)
	{
		// 检查基础碰撞
		if(Physics.CheckSphere(pos, 1f, LayerMask.GetMask("Wall", "Enemy")))
			return false;

		// 玩家额外检查
		if(isPlayer)
		{
			return !Physics.CheckSphere(pos, 1f, LayerMask.GetMask("Player"));
		}
		// 敌人检查玩家距离
		else
		{
			if(player == null) return false;
			if(Vector3.Distance(pos, player.transform.position) < playerSafeRadius)
				return false;
		}

		return true;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Vector3 center = new Vector3(
				(xMin + xMax) / 2,
				fixedY,
				(zMin + zMax) / 2
				);
		Vector3 size = new Vector3(
				xMax - xMin,
				0.1f,
				zMax - zMin
				);
		Gizmos.DrawWireCube(center, size);
	}
}
