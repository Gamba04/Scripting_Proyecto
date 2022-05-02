using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    private Player player;

    #region Init

    public void Init(Player player)
    {
        this.player = player;
    }

    #endregion

}