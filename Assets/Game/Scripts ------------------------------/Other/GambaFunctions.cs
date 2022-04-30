using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GambaFunctions
{

    #region Math

    public static float GetAngle(Vector2 point)
    {
        float pi = Mathf.PI;

        float x = point.x;
        float y = point.y;

        float r;

        if (x > 0)
        {
            if (y > 0) // Cuadrant: 1
            {
                r = Mathf.Atan(y / x);
            }
            else if (y < 0) // Cuadrant: 4
            {
                r = pi * 3 / 2f + (pi * 3 / 2f - (pi - Mathf.Atan(y / x)));
            }
            else // Right
            {
                r = 0;
            }
        }
        else if (x < 0)
        {
            if (y > 0) // Cuadrant: 2
            {
                r = pi * 1 / 2f + (pi * 1 / 2f + Mathf.Atan(y / x));

            }
            else if (y < 0) // Cuadrant: 3
            {
                r = pi + Mathf.Atan(y / x);

            }
            else // Left
            {
                r = pi;
            }
        }
        else
        {
            if (y > 0) // Up
            {
                r = pi * 1 / 2f;
            }
            else if (y < 0) // Down
            {
                r = pi * 3 / 2f;
            }
            else // Zero
            {
                r = 0;
            }
        }

        return r;
    }

    public static Vector2 AngleToVector(float angle) => new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

    public static Vector2 Bounce(Vector2 normal, Vector2 direction)
    {
        normal.Normalize();
        direction.Normalize();

        float dotNormalDirection = Vector2.Dot(direction.normalized, normal);

        Vector2 dashYComponent = normal * Mathf.Abs(dotNormalDirection);

        Vector2 dashXComponent;

        if (dotNormalDirection > 0)
        {
            dashXComponent = direction - dashYComponent;
        }
        else
        {
            dashXComponent = direction + dashYComponent;
        }

        return (dashXComponent + dashYComponent);
    }

    public static Vector2 Perpendicular(Vector2 direction) => new Vector2(direction.y, -direction.x);

    public static Color GetColorWithAlpha(Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);

    public static Vector2 VectorMult(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);

    public static Vector3 VectorMult(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);

    #endregion

    #region Gizmos

    public static void GizmosDrawArrow(Vector2 origin, Vector2 direction)
    {
        Vector2 head = origin + direction;
        Gizmos.DrawLine(origin, head);

        Vector2 perpendicular = new Vector2(direction.y, -direction.x);
        Gizmos.DrawLine(head, head + perpendicular * 0.1f - direction * 0.2f);
        Gizmos.DrawLine(head, head - perpendicular * 0.1f - direction * 0.2f);
    }

    public static void GizmosDrawArrow(Vector2 origin, Vector2 direction, Vector2 headSize)
    {
        Vector2 head = origin + direction;
        Gizmos.DrawLine(origin, head);

        Vector2 perpendicular = Perpendicular(direction);

        Gizmos.DrawLine(head, head + perpendicular.normalized * headSize / 2f - direction.normalized * headSize);
        Gizmos.DrawLine(head, head - perpendicular.normalized * headSize / 2f - direction.normalized * headSize);
    }

    public static void GizmosDrawPointedLine(Vector3 from, Vector3 to, float separation, int maxIters = 500)
    {
        float distance = (to - from).magnitude;
        float distanceToA = 0;
        float distanceToB = 0;
        int iter = 0;

        while (distanceToB < distance && iter < maxIters)
        {
            Vector3 offset = Vector3.zero;
            Vector3 pointA = from + (to - from).normalized * (iter) * separation;
            Vector3 pointB = from + (to - from).normalized * (iter + 0.5f) * separation;

            distanceToA = (pointA - from).magnitude;
            distanceToB = (pointB - from).magnitude;

            if (distanceToA < distance)
            {
                if (distanceToB > distance)
                {
                    offset = to - pointB;
                }

                Gizmos.DrawLine(pointA, pointB + offset);
            }

            iter++;
        }
    }

    #endregion

    #region Lists

    public static void ResizeList<T>(ref List<T> list, int newLenght) where T : new()
    {
        if (list == null)
        {
            list = new List<T>();
        }

        if (list.Count != newLenght)
        {
            if (newLenght >= 0)
            {
                List<T> newList = new List<T>();

                for (int i = 0; i < newLenght; i++)
                {
                    if (i < list.Count)
                    {
                        newList.Add(list[i]);
                    }
                    else
                    {
                        newList.Add(new T());
                    }
                }

                list = newList;
            }
        }
    }

    /// <summary> Resize list to enum lenght. </summary>
    public static void ResizeList<T, E>(ref List<T> list)
        where T : new()
        where E : Enum
    {
        int targetLenght = Enum.GetValues(typeof(E)).Length;

        ResizeList(ref list, targetLenght);
    }

    /// <summary> For types that without basic constructor. </summary>
    public static void ResizeListEmpty<T>(ref List<T> list, int newLenght)
    {
        if (list == null)
        {
            list = new List<T>();
        }

        if (newLenght >= 0)
        {
            List<T> newList = new List<T>();

            for (int i = 0; i < newLenght; i++)
            {
                if (i < list.Count)
                {
                    newList.Add(list[i]);
                }
                else
                {
                    newList.Add(default);
                }
            }

            list = newList;
        }
    }

    /// <summary> Resize list to enum lenght for types without basic constructor. </summary>
    public static void ResizeListEmpty<T, E>(ref List<T> list) where E : Enum
    {
        int targetLenght = Enum.GetValues(typeof(E)).Length;

        ResizeListEmpty(ref list, targetLenght);
    }

    #endregion

    #region Editor

    public static void DestroyInEditor(UnityEngine.Object @object)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += () =>
        {
            MonoBehaviour.DestroyImmediate(@object);
        };
#endif
    }

    #endregion

}