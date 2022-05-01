using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int health;

    #region Public Methods

    public void Attack(Character character)
    {
        character.ReceiveAttack(health, OnKill);
    }

    public void ReceiveAttack(int damage, Action<int> onKill)
    {
        int originalHealth = health;

        health -= damage;

        if (health <= 0)
        {
            Die();

            onKill?.Invoke(originalHealth);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void OnKill(int health)
    {
        this.health += health;
    }

    private void Die()
    {

    }

    #endregion

}