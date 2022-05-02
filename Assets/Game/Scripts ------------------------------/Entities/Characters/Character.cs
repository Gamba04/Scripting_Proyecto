using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    [Header("Character")]
    [Header("Components")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    protected TextMeshPro healthText;

    [Header("Settings")]
    [SerializeField]
    private float attackDelay = 1;
    [SerializeField]
    private float deathDelay = 2;

    protected int health;

    public event Action onDeath;

    public int Helath => health;

    #region Public Methods

    public void Attack(Character character, Action onFinishAttack = null)
    {
        anim.SetTrigger("Attack");

        Timer.CallOnDelay(() => character.ReceiveAttack(health, onFinishAttack, OnKill), attackDelay, $"{name} attack");
    }

    public void ReceiveAttack(int damage, Action onFinishAttack, Action<int> onKill)
    {
        int originalHealth = health;

        health -= damage;

        OnDamageTaken();

        if (health <= 0)
        {
            Die(() =>
            {
                onKill?.Invoke(originalHealth);
                onFinishAttack?.Invoke();
            });
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void OnDamageTaken()
    {
        healthText.text = health.ToString();
    }

    private void OnKill(int health)
    {
        this.health += health;
        healthText.text = this.health.ToString();
    }

    private void Die(Action onFinishDeath = null)
    {
        anim.SetBool("Dead", true);

        Timer.CallOnDelay(() =>
        {
            OnFinishDeath();

            onFinishDeath?.Invoke();
        }, deathDelay, $"{name} death");
    }

    private void OnFinishDeath()
    {
        onDeath?.Invoke();

        healthText.gameObject.SetActive(false);
    }

    #endregion

}