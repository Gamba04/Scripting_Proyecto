using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Setup/Game Data", order = 0)]
public class GameDataScrObj : ScriptableObject
{
    [ReadOnly, SerializeField]
    private LevelsScrObj levels;

    [Space]
    [SerializeField]
    private int level;

    public int Level => level;

    /// <summary> Go to next level if possible or loop to first. </summary>
    public void NextLevel()
    {
        if (level < levels.LevelCount - 1) // Has next level
        {
            level++;
        }
        else
        {
            level = 0;
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        level = Mathf.Clamp(level, 0, levels.LevelCount - 1);
    }

#endif

    #endregion

}