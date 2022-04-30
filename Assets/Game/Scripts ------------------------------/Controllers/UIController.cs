using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image fade;

    [Header("Settings")]
    [SerializeField]
    private TransitionColor fadeTransition;
    [SerializeField]
    private Color fadeColor = Color.white;

    public static Canvas Canvas => Instance.canvas;

    #region Start

    #region Singleton

    private static UIController instance = null;

    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                var sceneResult = FindObjectOfType<UIController>();

                if (sceneResult != null)
                {
                    instance = sceneResult;
                }
                else
                {
                    GameObject obj = new GameObject($"{GetTypeName(instance)}_Instance");

                    instance = obj.AddComponent<UIController>();
                }
            }

            return instance;
        }
    }

    private static string GetTypeName<T>(T obj) => typeof(T).Name;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        OnStart();
    }

    #endregion

    private void OnStart()
    {
        SetFade(true, true);
        SetFade(false);
    }

    #endregion

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        TransitionsUpdate();
    }

    private void TransitionsUpdate()
    {
        fadeTransition.UpdateTransition();

        if (fadeTransition.IsOnTransition)
        {
            fade.color = fadeTransition.value;
        }
    }

    #endregion

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    #region General Methods

    public static Vector3 ScreenToCanvasPos(Vector2 position)
    {
        Canvas canvas = Canvas;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 newPos = canvas.worldCamera.ScreenToWorldPoint(position);
            newPos.z = canvas.transform.position.z;

            return newPos;
        }
        else
        {
            return position;
        }
    }

    public static Vector2 CanvasToScreenPos(Vector3 position)
    {
        Canvas canvas = Canvas;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            return canvas.worldCamera.WorldToScreenPoint(position);
        }
        else
        {
            return position;
        }
    }

    public static Vector3 ScreenToCanvasVector(Vector2 vector)
    {
        Canvas canvas = Canvas;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector2 newVec = vector / Screen.height * canvas.worldCamera.orthographicSize * 2;

            return newVec;
        }
        else
        {
            return vector;
        }
    }

    public static void SetFade(bool visible, bool instant = false, Action onTransitionEnd = null)
    {
        Action onNewTransitionEnd = onTransitionEnd;

        if (visible)
        {
            Instance.fade.gameObject.SetActive(true);
        }
        else
        {
            onNewTransitionEnd = () =>
            {
                onTransitionEnd?.Invoke();
                Instance.fade.gameObject.SetActive(false);
            };
        }

        if (instant)
        {
            Color color = visible ? Instance.fadeColor : GambaFunctions.GetColorWithAlpha(Instance.fadeColor, 0);

            Instance.fadeTransition.value = color;
            Instance.fade.color = color;

            onNewTransitionEnd?.Invoke();
        }
        else Instance.fadeTransition.StartTransition(visible ? Instance.fadeColor : GambaFunctions.GetColorWithAlpha(Instance.fadeColor, 0), onNewTransitionEnd);
    }

    private new static void print(object text) => GameManager.print(text);

    #endregion

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    #region Static Methods



    #endregion

}