using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public void Init(EnemyInfo info)
    {
        health = info.health;
    }
}