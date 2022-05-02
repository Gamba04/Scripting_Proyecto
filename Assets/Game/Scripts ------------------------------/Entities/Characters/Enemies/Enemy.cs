using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : Character
{
    [Header("Enemy")]
    [Header("Components")]
    [SerializeField]
    private SpriteRenderer sr;

    public void Init(EnemyInfo info, int index)
    {
        health = info.health;
        healthText.text = health.ToString();

        sr.sortingOrder = -index;
    }
}