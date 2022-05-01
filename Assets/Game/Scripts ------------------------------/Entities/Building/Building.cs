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

    [Header("Settings")]
    [SerializeField]
    private float floorSeparation = 5;

    private List<Room> rooms;

    #region Init

    public void Init(Vector3 pivot)
    {
        // base building
    }

    public void Init(Vector3 pivot, LevelInfo info)
    {
        for (int i = 0; i < info.rooms.Count; i++)
        {
            Room room = CreateRoom(info.rooms[i], pivot, i);

            rooms.Add(room);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private Room CreateRoom(RoomInfo info, Vector3 pivot, int index)
    {
        Room room = Instantiate(roomPrefab, roomParent);
        room.name = $"Room {index}";
        room.transform.position = pivot + Vector3.up * index * floorSeparation;

        room.Init(info);

        return room;
    }

    #endregion

}