using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Serializable]
    private class Request
    {
        [SerializeField, HideInInspector] private string name;

        public float timer;
        public Action action;
        public Action<float> onUpdateValue;
        public bool unscaled;

        public bool abort;
        public Action cancelAction;

        public Request(float timer, Action action, bool unscaled)
        {
            this.timer = timer;
            this.action = action;
            this.unscaled = unscaled;
        }

        public Request(float timer, Action action, Action<float> onUpdateValue, bool unscaled)
        {
            this.timer = timer;
            this.action = action;
            this.unscaled = unscaled;
            this.onUpdateValue = onUpdateValue;
        }

        public void SetTimer(float value)
        {
            timer = value;
            onUpdateValue?.Invoke(value);
        }

        public void SetCancelAction(ref Action cancel)
        {
            cancel += Cancel;
            cancelAction = cancel;
        }

        private void Cancel()
        {
            abort = true;
            cancelAction -= Cancel;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
    }

    [SerializeField]
    private List<Request> requests = new List<Request>();

    #region Singleton

    private static Timer instance = null;

    public static Timer Instance
    {
        get
        {
            if (instance == null)
            {
                var sceneResult = FindObjectOfType<Timer>();

                if (sceneResult != null)
                {
                    instance = sceneResult;
                }
                else
                {
                    GameObject obj = new GameObject($"{GetTypeName(instance)}_Instance");

                    instance = obj.AddComponent<Timer>();
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
    }

    #endregion

    #region Update

    private void Update()
    {
        TimerManagement();
    }

    private void TimerManagement()
    {
        for (int i = 0; i < requests.Count; i++)
        {
            if (!requests[i].abort)
            {
                float timer = requests[i].timer;

                if (requests[i].unscaled)
                {
                    ReduceCooldownUnscaled(ref timer, requests[i].action);
                }
                else
                {
                    ReduceCooldown(ref timer, requests[i].action);
                }

                requests[i].SetTimer(timer);
            }
        }

        for (int i = requests.Count - 1; i >= 0; i--)
        {
            if (requests[i].timer <= 0 || requests[i].abort)
            {
                requests.RemoveAt(i);
            }
        }
    }

    #endregion

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    private static void CallOnDelayInternal(Action action, bool unscaled, float delay, Action<float> onUpdateValue, string optionalName)
    {
        if (delay > 0)
        {
            if (Instance.requests == null)
            {
                Instance.requests = new List<Request>();
            }

            Request request = new Request(delay, action, onUpdateValue, unscaled);
            request.SetName(optionalName);

            Instance.requests.Add(request);
        }
        else
        {
            action?.Invoke();
        }
    }

    private static void CallOnDelayInternal(Action action, bool unscaled, float delay, ref Action cancelAction, Action<float> onUpdateValue, string optionalName)
    {
        if (delay > 0)
        {
            if (Instance.requests == null)
            {
                Instance.requests = new List<Request>();
            }

            Request request = new Request(delay, action, onUpdateValue, unscaled);
            request.SetName(optionalName);
            request.SetCancelAction(ref cancelAction);

            Instance.requests.Add(request);
        }
        else
        {
            action?.Invoke();
        }
    }

    private static void ReduceCooldownInternal(ref float value, bool unscaled, Action endingAction)
    {
        if (value > 0)
        {
            value -= unscaled ? Time.unscaledDeltaTime : Time.deltaTime;

            if (value <= 0)
            {
                value = 0;

                endingAction?.Invoke();
            }
        }
        else if (value < 0)
        {
            value = 0;
        }
    }

    /// <summary> Call an Action after a period of time. </summary>
    public static void CallOnDelay(Action action, float delay, string optionalName = "") => CallOnDelayInternal(action, false, delay, null, optionalName);

    /// <summary> Call an Action after a period of time, with a cancellation Action. </summary>
    public static void CallOnDelay(Action action, float delay, ref Action cancelAction, string optionalName = "") => CallOnDelayInternal(action, false, delay, ref cancelAction, null, optionalName);

    /// <summary> Call an Action after a period of time, with a cancellation Action. </summary>
    public static void CallOnDelay(Action action, float delay, ref Action cancelAction, Action<float> onUpdateValue, string optionalName = "") => CallOnDelayInternal(action, false, delay, ref cancelAction, onUpdateValue, optionalName);

    /// <summary> Call an Action after a period of unscaled time. </summary>
    public static void CallOnDelayUnscaled(Action action, float delay, string optionalName = "") => CallOnDelayInternal(action, true, delay, null, optionalName);

    /// <summary> Call an Action after a period of unscaled time, with a cancellation Action. </summary>
    public static void CallOnDelayUnscaled(Action action, float delay, ref Action cancelAction, string optionalName = "") => CallOnDelayInternal(action, true, delay, ref cancelAction, null, optionalName);

    /// <summary> Call an Action after a period of unscaled time, with a cancellation Action. </summary>
    public static void CallOnDelayUnscaled(Action action, float delay, ref Action cancelAction, Action<float> onUpdateValue, string optionalName = "") => CallOnDelayInternal(action, true, delay, ref cancelAction, onUpdateValue, optionalName);

    /// <summary> Reduce a variable over time and call an Action if it reaches 0. </summary>
    public static void ReduceCooldown(ref float value, Action endingAction = null) => ReduceCooldownInternal(ref value, false, endingAction);

    /// <summary> Reduce a variable over unscaled time and call an Action if it reaches 0. </summary>
    public static void ReduceCooldownUnscaled(ref float value, Action endingAction = null) => ReduceCooldownInternal(ref value, true, endingAction);

    #endregion

}