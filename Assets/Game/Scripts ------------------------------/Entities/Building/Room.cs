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
    [SerializeField]
    private Vector2 playerPivot;

    private Stack<Enemy> enemies = new Stack<Enemy>();

    public Vector2 PlayerPosition => (Vector2)transform.position + playerPivot;

    #region Init

    public void Init(RoomInfo info)
    {
        if (info.enemies == null) // Player building's room
        {
            Destroy(enemyParent.gameObject);
            return;
        }

        for (int i = 0; i < info.enemies.Count; i++)
        {
            Enemy enemy = CreateEnemy(info.enemies[i], i);

            enemies.Push(enemy);
        }

        EventsInit();
    }

    private void EventsInit()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.onDeath += OnEnemyDeath;
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public Enemy GetFirstEnemy() => enemies.Count > 0 ? enemies.Peek() : null;

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private Enemy CreateEnemy(EnemyInfo info, int index)
    {
        Enemy enemy = Instantiate(enemyPrefab, enemyParent);
        enemy.name = $"Enemy {index + 1}";
        enemy.transform.position = transform.position + (Vector3)enemiesPivot + Vector3.left * index * enemiesSeparation;

        enemy.Init(info, index);

        return enemy;
    }

    private void OnEnemyDeath()
    {
        if (enemies.Count > 0) enemies.Pop();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + enemiesPivot, enemiesSeparation / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((Vector2)transform.position + playerPivot, 0.5f);
    }

#endif

    #endregion

}