using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameDataScrObj gameData;
    [SerializeField]
    private bool debugs = true;

    [Header("Components")]
    [SerializeField]
    private LevelController levelController;

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
        levelController.Init(gameData.Level);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {

    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public new static void print(object text)
    {

#if UNITY_EDITOR

        if (Instance.debugs)
        {
            MonoBehaviour.print(text);
        }

#endif

    }

    #endregion

}