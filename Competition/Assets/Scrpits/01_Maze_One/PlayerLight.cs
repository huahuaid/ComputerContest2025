using UnityEngine;
using System.Collections.Generic;

public class PlayerLight : MonoBehaviour
{
    [Header("光源设置")]
    public float lightRadius = 5f;    // 光源半径
    public LayerMask enemyLayer;      // 敌人层级

    private HashSet<EnemyVisibility> litEnemies = new HashSet<EnemyVisibility>();

    void Update()
    {
        // 临时存储当前帧检测到的敌人
        HashSet<EnemyVisibility> currentFrameEnemies = new HashSet<EnemyVisibility>();

        // 检测范围内的敌人
        Collider[] enemies = Physics.OverlapSphere(transform.position, lightRadius, enemyLayer);
        
        foreach (Collider enemyCol in enemies)
        {
            // 光线投射检测遮挡物（可选）
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

        // 处理离开范围的敌人
        foreach (EnemyVisibility enemy in litEnemies)
        {
            if (!currentFrameEnemies.Contains(enemy) && enemy != null)
            {
                enemy.SetVisible(false);
            }
        }

        // 更新已点亮敌人集合
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
