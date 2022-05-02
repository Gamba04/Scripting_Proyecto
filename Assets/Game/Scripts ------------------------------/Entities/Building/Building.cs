using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Room roomPrefab;
    [SerializeField]
    private Transform roomParent;
    [SerializeField]
    private List<SpriteRenderer> backgroundSprites;

    [Header("Settings")]
    [SerializeField]
    private float floorSeparation = 5;

    private List<Room> rooms = new List<Room>();

    #region Init

    /// <summary> Base building with 1 room. </summary>
    public void Init()
    {
        SetFloors(1, true);

        Room room = CreateRoom(new RoomInfo(), 0);

        rooms.Add(room);
    }

    /// <summary> Custom building based on info. </summary>
    public void Init(LevelInfo info)
    {
        SetFloors(info.rooms.Count, true);

        for (int i = 0; i < info.rooms.Count; i++)
        {
            Room room = CreateRoom(info.rooms[i], i);

            rooms.Add(room);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void SetFloors(int amount, bool instant, Action onFinish = null)
    {
        if (instant)
        {
            foreach (SpriteRenderer sprite in backgroundSprites) sprite.size = new Vector2(sprite.size.x, amount);

            onFinish?.Invoke();
        }
        else
        {
            // animation
        }
    }

    private Room CreateRoom(RoomInfo info, int index)
    {
        Room room = Instantiate(roomPrefab, roomParent);
        room.name = $"Room {index + 1}";
        room.transform.position = transform.position + Vector3.up * index * floorSeparation;

        room.Init(info);

        return room;
    }

    #endregion

}