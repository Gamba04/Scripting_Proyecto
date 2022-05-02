using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    private Player player;

    private Vector3 lastValidPosition;

    public event Action onAttackFinished;

    #region Init

    public void Init(Player player)
    {
        this.player = player;

        lastValidPosition = player.transform.position;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void OnResetPlayerPosition()
    {
        player.transform.position = lastValidPosition;
    }

    public void OnStartRoomAttack(Room room)
    {
        player.transform.position = room.PlayerPosition;
        lastValidPosition = player.transform.position;

        AttackRoom(room);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void AttackRoom(Room room)
    {
        Enemy enemy = room.GetFirstEnemy();

        // adjust player position

        if (enemy != null) // Attack enemy
        {
            float attackResult = player.Helath - enemy.Helath;

            print(attackResult);

            if (attackResult > 0) // Player survives
            {
                player.Attack(enemy, () => AttackRoom(room));
                print("Player attacks");
            }
            else // Enemy survives
            {
                enemy.Attack(player);
                print("Enemy attacks");
            }
        }
        else // No more enemies
        {
            FinishAttack();
        }
    }

    private void FinishAttack()
    {
        onAttackFinished?.Invoke();
    }

    public new static void print(object message) => GameManager.print(message);

    #endregion

}