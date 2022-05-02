using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Player player;

    private bool active;
    private bool isDragging;

    public event Action onStartDrag;
    public event Action onResetDrag;
    public event Action<Room> onAttackRoom;

    #region Init

    public void Init()
    {
        SetEnabled(true);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void SetEnabled(bool active)
    {
        this.active = active;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        if (active)
        {
            if (!isDragging)
            {
                DetectionUpdate();
            }
            else
            {
                DraggingUpdate();
            }
        }
    }

    private void DetectionUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player = CheckDetection<Player>(LayerMask.GetMask("Player"));

            if (player != null)
            {
                StartDrag();
            }
        }
    }

    private void DraggingUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
            return;
        }

        Vector2 mouseWorldPos = GameManager.Camera.ScreenToWorldPoint(Input.mousePosition);

        player.transform.position = mouseWorldPos;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void StartDrag()
    {
        print("Start drag");

        isDragging = true;

        DraggingUpdate();

        player.SetDraggingState(Player.DraggingState.Dragging);

        onStartDrag?.Invoke();
    }

    private void EndDrag()
    {
        print("End drag");

        isDragging = false;
        SetEnabled(false);

        player.SetDraggingState(Player.DraggingState.Normal);

        Room targetRoom = CheckDetection<Room>(LayerMask.GetMask("Room"));

        if (targetRoom != null)
        {
            onAttackRoom?.Invoke(targetRoom);
        }
        else
        {
            SetEnabled(true);

            onResetDrag?.Invoke();
        }

        player = null;
    }

    private T CheckDetection<T>(Vector2 screenPos, int layerMask = ~0) where T : MonoBehaviour
    {
        Vector2 worldPos = GameManager.Camera.ScreenToWorldPoint(screenPos);

        Collider2D result = Physics2D.OverlapPoint(worldPos, layerMask);

        if (result == null) return null;

        T target = result.attachedRigidbody?.GetComponent<T>();

        if (target != null)
        {
            return target;
        }

        return null;
    }

    private T CheckDetection<T>(int layerMask = ~0) where T : MonoBehaviour => CheckDetection<T>(Input.mousePosition, layerMask);

    public new static void print(object message) => GameManager.print(message);

    #endregion

}