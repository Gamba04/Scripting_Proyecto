using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameDataScrObj gameData;
    [SerializeField]
    private bool debugs = true;

    [Header("Components")]
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private LevelController levelController;
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private FightController fightController;
    [SerializeField]
    private Player player;

    [Header("Settings")]
    [SerializeField]
    private int playerStartHealth = 10;

    public static Camera Camera => Instance.camera;

    #region Start

    #region Singleton

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var sceneResult = FindObjectOfType<GameManager>();

                if (sceneResult != null)
                {
                    instance = sceneResult;
                }
                else
                {
                    GameObject obj = new GameObject($"{GetTypeName(instance)}_Instance");
                    instance = obj.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    private static string GetTypeName<T>(T obj) => typeof(T).Name;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        OnStart();
    }

    #endregion

    private void OnStart()
    {
        player.Init(playerStartHealth);

        levelController.Init(gameData.Level, player);
        inputController.Init();
        fightController.Init(player);

        EventsStart();
    }

    private void EventsStart()
    {
        player.onDeath += OnPlayerDeath;

        inputController.onStartDrag += OnInputStartDrag;
        inputController.onResetDrag += OnInputResetDrag;
        inputController.onAttackRoom += OnInputAttackRoom;

        fightController.onAttackFinished += OnAttackFinished;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public new static void print(object message)
    {

#if UNITY_EDITOR

        if (Instance.debugs)
        {
            MonoBehaviour.print(message);
        }

#endif

    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void OnInputStartDrag()
    {

    }

    private void OnInputResetDrag()
    {
        fightController.OnResetPlayerPosition();
    }

    private void OnInputAttackRoom(Room room)
    {
        fightController.OnStartRoomAttack(room);
    }

    private void OnAttackFinished()
    {
        inputController.SetEnabled(true);
    }

    private void OnPlayerDeath()
    {
        // death
        print("Player died");

        UIController.SetFade(true, onTransitionEnd: RestartLevel);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

}