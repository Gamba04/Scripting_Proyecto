using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public enum DraggingState
    {
        Normal,
        Dragging,
        //Hover
    }

    [Header("Player")]
    [Header("Components")]
    [SerializeField]
    private SpriteRenderer sr;

    [Header("Settings")]
    [SerializeField]
    [Range(0, 1)]
    private float draggingAlpha;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private DraggingState draggingState;

    #region Init

    public void Init(int startHealth)
    {
        health = startHealth;
        healthText.text = health.ToString();

        SetDraggingState(DraggingState.Normal);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void SetDraggingState(DraggingState draggingState)
    {
        this.draggingState = draggingState;

        switch (draggingState)
        {
            case DraggingState.Normal:
                sr.color = GambaFunctions.GetColorWithAlpha(sr.color, 1);
                break;

            case DraggingState.Dragging:
                sr.color = GambaFunctions.GetColorWithAlpha(sr.color, draggingAlpha);
                break;
        }
    }

    #endregion

}