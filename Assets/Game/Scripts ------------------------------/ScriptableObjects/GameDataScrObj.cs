using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Setup/Game Data", order = 0)]
public class GameDataScrObj : ScriptableObject
{
    [SerializeField]
    private int level;

    public int Level => level;

    public void NextLevel()
    {
        level++;
    }
}