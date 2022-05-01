using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Enemy enemyPrefab;
    [SerializeField]
    private Transform enemyParent;

    [Header("Settings")]
    [SerializeField]
    private Vector2 enemiesPivot;
    [SerializeField]
    private float enemiesSeparation;

    private Queue<Enemy> enemies = new Queue<Enemy>();

    #region Public Methods

    public void CreateRoom(RoomInfo info)
    {
        for (int i = 0; i < info.enemies.Count; i++)
        {
            Enemy enemy = CreateEnemy(info.enemies[i], i);

            enemies.Enqueue(enemy);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private Enemy CreateEnemy(EnemyInfo info, int index)
    {
        Enemy enemy = Instantiate(enemyPrefab, enemyParent);
        enemy.name = $"Enemy {index}";
        enemy.transform.position = transform.position + (Vector3)enemiesPivot + Vector3.left * index * enemiesSeparation;

        enemy.Init(info);

        return enemy;
    }

    #endregion

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemiesPivot, 0.5f);
    }

#endif

    #endregion

}