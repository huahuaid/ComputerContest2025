using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnArea
{
    public float xMin = -190f; 
    public float xMax = -90f;
    public float zMin = -6f;
    public float zMax = 93f;
    public float fixedY = 7f;
}

public class MapGenerator : MonoBehaviour
{
    [Header("墙壁设置")]
    public GameObject wallPrefab;
    public int maxWalls = 50;
    public float minDistance = 3f; // 墙体之间的最小距离
    public int maxAttempts = 30;

    [Header("生成范围")]
    public SpawnArea spawnArea;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private int[] possibleRotations = { 0, 90, 180, 270 };

    void Start()
    {
        GenerateRandomMap();
    }

    void GenerateRandomMap()
    {
        for (int i = 0; i < maxWalls; i++)
        {
            TrySpawnWall(i);
        }
    }

    void TrySpawnWall(int attempt)
    {
        Vector3 newPos;
        bool validPosition = false;
        int attempts = 0;

        do
        {
            // 生成随机位置
            newPos = new Vector3(
                Random.Range(spawnArea.xMin, spawnArea.xMax),
                spawnArea.fixedY,
                Random.Range(spawnArea.zMin, spawnArea.zMax)
            );

            // 验证位置有效性
            validPosition = IsPositionValid(newPos);
            attempts++;

        } while (!validPosition && attempts < maxAttempts);

        if (validPosition)
        {
            // 生成随机旋转
            Quaternion newRot = Quaternion.Euler(0, possibleRotations[Random.Range(0, 4)], 0);

            Instantiate(wallPrefab, newPos, newRot);
            spawnedPositions.Add(newPos);
        }
        else
        {
            Debug.LogWarning($"第{attempt + 1}个墙体生成失败，已达最大尝试次数");
        }
    }

    bool IsPositionValid(Vector3 checkPos)
    {
        // 检查与已有墙体的距离
        foreach (Vector3 pos in spawnedPositions)
        {
            if (Vector3.Distance(checkPos, pos) < minDistance)
            {
                return false;
            }
        }

        // 检查是否与 tag 为 "Item" 或 "Player" 的对象重叠
        Collider[] colliders = Physics.OverlapSphere(checkPos, minDistance);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Item") || col.CompareTag("Player"))
            {
                return false;
            }
        }

        return true;
    }

    // 可视化生成范围（编辑器用）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3(
            (spawnArea.xMin + spawnArea.xMax) / 2,
            spawnArea.fixedY,
            (spawnArea.zMin + spawnArea.zMax) / 2
        );
        Vector3 size = new Vector3(
            spawnArea.xMax - spawnArea.xMin,
            0.1f,
            spawnArea.zMax - spawnArea.zMin
        );
        Gizmos.DrawWireCube(center, size);
    }
}
