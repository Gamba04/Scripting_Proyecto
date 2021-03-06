using System;
using System.Collections.Generic;
using UnityEngine;

#region Serializable

[Serializable]
public class LevelInfo
{
    [SerializeField, HideInInspector] private string name;

    public List<RoomInfo> rooms;

    public void SetName(int index, int totalHealth)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetName(i);
        }

        name = $"Level {index + 1} (+{totalHealth})";
    }

    public int GetTotalHealth()
    {
        int health = 0;

        foreach (RoomInfo room in rooms) health += room.GetTotalHealth();

        return health;
    }
}

[Serializable]
public class RoomInfo
{
    [SerializeField, HideInInspector] private string name;

    public List<EnemyInfo> enemies;

    public void SetName(int index)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetName(i);
        }

        name = $"Room {index + 1} {new string('-', 200)}";
    }

    public int GetTotalHealth()
    {
        int health = 0;

        foreach (EnemyInfo enemy in enemies) health += enemy.health;

        return health;
    }
}

[Serializable]
public class EnemyInfo
{
    [SerializeField, HideInInspector] private string name;

    public int health;
    // visual info

    public void SetName(int index)
    {
        health = Mathf.Max(health, 0);

        name = $"Enemy {index + 1}";
    }
}

#endregion

[CreateAssetMenu(fileName = "Levels", menuName = "Setup/Levels Data", order = 1)]
public class LevelsScrObj : ScriptableObject
{
    [SerializeField]
    private List<LevelInfo> levels = new List<LevelInfo>();

    public int LevelCount => levels.Count;

    #region Public Methods

    public LevelInfo GetLevel(int index) => levels[index];

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        int totalHealth = 0;

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetName(i, totalHealth);

            totalHealth += levels[i].GetTotalHealth();
        }
    }

#endif

    #endregion

}