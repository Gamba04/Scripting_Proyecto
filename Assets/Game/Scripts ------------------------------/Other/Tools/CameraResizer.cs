using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResizer : MonoBehaviour
{
    [Serializable]
    private class AspectRatio
    {
        [SerializeField, HideInInspector] private string name;

        public Vector2 ratio;
        public float size;

        public float GetRatio() => ratio.x / ratio.y;

        public AspectRatio()
        {
            ratio = new Vector2(16, 9);
            size = 5;
        }

        public AspectRatio(Vector2 ratio, float size)
        {
            this.ratio = ratio;
            this.size = size;
        }

        public void SetName()
        {
            name = $"{ratio.x}:{ratio.y} - {size}";
        }
    }

    [SerializeField]
    private bool webGL;

    [Header("Components")]
    [SerializeField]
    private new Camera camera;

    [Header("Settings")]
    [SerializeField]
    private List<AspectRatio> aspectRatios = new List<AspectRatio>();

    [Header("Info")]
    [ReadOnly, SerializeField]
    private float currentRatio;
    [SerializeField]
    private AnimationCurve curve;

    private void Start()
    {
        if (Application.isPlaying)
        {
            if (!webGL)
            {
                GenerateCurve();
                ResizeCamera();
            }
            else
            {
                StartCoroutine(StartOnFrameEnd());
            }
        }
        else
        {
            // Default settings
            if (aspectRatios.Count == 0)
            {
                aspectRatios.Add(new AspectRatio(new Vector2(3, 4), 4));
                aspectRatios.Add(new AspectRatio(new Vector2(9, 16), 5));
                aspectRatios.Add(new AspectRatio(new Vector2(9, 21), 7));
            }
        }
    }

    #region Other Methods

    private void GenerateCurve()
    {
        curve = new AnimationCurve();

        for (int i = 0; i < aspectRatios.Count; i++)
        {
            curve.AddKey(new Keyframe(aspectRatios[i].GetRatio(), aspectRatios[i].size));
        }

        // Smooth tangents
        for (int i = 0; i < curve.length; i++)
        {
            int prev = i - 1;
            int next = i + 1;

            Keyframe prevKey = curve.keys[prev < 0 ? 0 : prev];
            Keyframe nextKey = curve.keys[next > curve.length - 1 ? curve.length - 1 : next];
            Keyframe currKey = curve.keys[i];

            float inTangent = (currKey.value - prevKey.value) / (currKey.time - prevKey.time);
            float outTangent = (nextKey.value - currKey.value) / (nextKey.time - currKey.time);

            if (prev < 0) inTangent = outTangent;
            if (next > curve.length - 1) outTangent = inTangent;

            float average = (outTangent + inTangent) / 2f;

            currKey.inTangent = average;
            currKey.outTangent = average;

            curve.MoveKey(i, currKey);
        }

        currentRatio = camera ? camera.aspect : -1;
    }

    private void ResizeCamera()
    {
        if (camera != null && curve.keys.Length > 0)
        {
            camera.orthographicSize = curve.Evaluate(currentRatio);
        }
    }

    private IEnumerator StartOnFrameEnd()
    {
        yield return new WaitForEndOfFrame();

        GenerateCurve();
        ResizeCamera();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void Update()
    {
        if (!Application.isPlaying)
        {
            for (int i = 0; i < aspectRatios.Count; i++)
            {
                aspectRatios[i].SetName();
            }

            if (camera == null)
            {
                camera = GetComponent<Camera>();
            }

            GenerateCurve();
            ResizeCamera();
        }
    }

#endif

    #endregion

}