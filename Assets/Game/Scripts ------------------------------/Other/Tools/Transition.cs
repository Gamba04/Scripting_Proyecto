using System;
using UnityEngine;
using Transitions;

[Serializable] public class Transition : Transition<float> { }

[Serializable] public class TransitionColor : Transition<Color> { }

[Serializable] public class TransitionVector2 : Transition<Vector2> { }

[Serializable] public class TransitionVector3 : Transition<Vector3> { }

[Serializable] public class TransitionQuaternion : Transition<Quaternion> { }

namespace Transitions
{
    [Serializable]
    public class Transition<T> where T : struct
    {
        [SerializeField]
        private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        [SerializeField]
        private float duration = 1;

        public T value;

        [SerializeField]
        private bool isOnTransition;

        private float transitionCooldown;
        private T targetValue;
        private T previousValue;

        private bool unscaled;
        private bool inverseCurve;
        private bool scaleDuration;

        private Action onTransitionEnd;

        public bool IsOnTransition => isOnTransition;

        public float Duration { get => duration; set => duration = value; }

        #region Public Methods

        public void StartTransition(T targetValue, Action onTransitionEnd = null) => StartTransitionInternal(targetValue, false, false, false, onTransitionEnd);

        public void StartTransition(T targetValue, bool inverseCurve, Action onTransitionEnd = null) => StartTransitionInternal(targetValue, false, inverseCurve, false, onTransitionEnd);

        public void StartTransition(T targetValue, bool inverseCurve, bool scaleDuration, Action onTransitionEnd = null) => StartTransitionInternal(targetValue, false, inverseCurve, scaleDuration, onTransitionEnd);

        public void StartTransitionUnscaled(T targetValue, Action onTransitionEnd = null) => StartTransitionInternal(targetValue, true, false, false, onTransitionEnd);

        public void StartTransitionUnscaled(T targetValue, bool inverseCurve, Action onTransitionEnd = null) => StartTransitionInternal(targetValue, true, inverseCurve, false, onTransitionEnd);

        public void StartTransitionUnscaled(T targetValue, bool inverseCurve, bool scaleDuration, Action onTransitionEnd = null) => StartTransitionInternal(targetValue, true, inverseCurve, scaleDuration, onTransitionEnd);

        public void UpdateTransition()
        {
            if (isOnTransition)
            {
                if (transitionCooldown == 0)
                {
                    isOnTransition = false;
                    onTransitionEnd?.Invoke();
                    onTransitionEnd = null;
                    return;
                }

                if (unscaled)
                {
                    Timer.ReduceCooldownUnscaled(ref transitionCooldown);
                }
                else
                {
                    Timer.ReduceCooldown(ref transitionCooldown);
                }

                float progress = 1 - transitionCooldown / duration;
                float interpolator = inverseCurve ? 1 - curve.Evaluate(1 - progress) : curve.Evaluate(progress);

                ProcessValue(interpolator);
            }
        }

        /// <summary> End transition and call onTransitionEnd </summary>
        public void EndTransition()
        {
            Cancel();

            onTransitionEnd?.Invoke();
            onTransitionEnd = null;
        }

        /// <summary> End transition without calling onTransitionEnd </summary>
        public void Cancel()
        {
            targetValue = value;
            isOnTransition = false;
            transitionCooldown = 0;
        }

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        #region Other

        private void StartTransitionInternal(T targetValue, bool unscaled, bool inverseCurve, bool scaleDuration, Action onTransitionEnd)
        {
            if (duration > 0 && !targetValue.Equals(value))
            {
                this.unscaled = unscaled;
                this.targetValue = targetValue;
                this.inverseCurve = inverseCurve;
                this.scaleDuration = scaleDuration;
                this.onTransitionEnd = onTransitionEnd;

                isOnTransition = true;
                transitionCooldown = scaleDuration ? duration * Distance(targetValue, value) : duration;
                previousValue = value;

                UpdateTransition();
            }
            else
            {
                value = targetValue;
                isOnTransition = false;
                onTransitionEnd?.Invoke();
            }
        }

        private void ProcessValue(float interpolator)
        {
            // Float value
            if (typeof(T).Equals(typeof(float)))
            {
                float previousFloatValue = ConvertTo<float>(previousValue);
                float targetFloatValue = ConvertTo<float>(targetValue);

                value = (T)(object)Mathf.Lerp(previousFloatValue, targetFloatValue, interpolator);
            }

            // Color value
            else if (typeof(T).Equals(typeof(Color)))
            {
                Color previousColorValue = ConvertTo<Color>(previousValue);
                Color targetColorValue = ConvertTo<Color>(targetValue);

                value = (T)(object)Color.Lerp(previousColorValue, targetColorValue, interpolator);
            }

            // Vector2 value
            else if (typeof(T).Equals(typeof(Vector2)))
            {
                Vector2 previousVector2Value = ConvertTo<Vector2>(previousValue);
                Vector2 targetVector2Value = ConvertTo<Vector2>(targetValue);

                value = (T)(object)Vector2.Lerp(previousVector2Value, targetVector2Value, interpolator);
            }

            // Vector3 value
            else if (typeof(T).Equals(typeof(Vector3)))
            {
                Vector3 previousVector3Value = ConvertTo<Vector3>(previousValue);
                Vector3 targetVector3Value = ConvertTo<Vector3>(targetValue);

                value = (T)(object)Vector3.Lerp(previousVector3Value, targetVector3Value, interpolator);
            }

            // Quaternion value
            else if (typeof(T).Equals(typeof(Quaternion)))
            {
                Quaternion previousQuaternionValue = ConvertTo<Quaternion>(previousValue);
                Quaternion targetQuaternionValue = ConvertTo<Quaternion>(targetValue);

                value = (T)(object)Quaternion.Slerp(previousQuaternionValue, targetQuaternionValue, interpolator);
            }

            // Not valid Type
            else
            {
                Debug.LogError($"Transition does not support {typeof(T).Name} type");
            }
        }

        private D ConvertTo<D>(T input)
        {
            if (input is D value)
            {
                return value;
            }

            return default;
        }

        private float Distance(T targetValue, T value)
        {
            // Float value
            if (typeof(T).Equals(typeof(float)))
            {
                float targetFloatValue = ConvertTo<float>(targetValue);
                float floatValue = ConvertTo<float>(value);

                return targetFloatValue - floatValue;
            }

            // Color value
            else if (typeof(T).Equals(typeof(Color)))
            {
                Color targetColorValue = ConvertTo<Color>(targetValue);
                Color colorValue = ConvertTo<Color>(value);

                Vector3 targetVector3Value = new Vector3(targetColorValue.r, targetColorValue.g, targetColorValue.b);
                Vector3 vector3Value = new Vector3(colorValue.r, colorValue.g, colorValue.b);

                return (targetVector3Value - vector3Value).magnitude;
            }

            // Vector2 value
            else if (typeof(T).Equals(typeof(Vector2)))
            {
                Vector2 targetVector2Value = ConvertTo<Vector2>(targetValue);
                Vector2 vector2Value = ConvertTo<Vector2>(value);

                return (targetVector2Value - vector2Value).magnitude;
            }

            // Vector3 value
            else if (typeof(T).Equals(typeof(Vector3)))
            {
                Vector3 targetVector3Value = ConvertTo<Vector3>(targetValue);
                Vector3 vector3Value = ConvertTo<Vector3>(value);

                return (targetVector3Value - vector3Value).magnitude;
            }

            // Quaternion value
            else if (typeof(T).Equals(typeof(Quaternion)))
            {
                Quaternion targetQuaternionValue = ConvertTo<Quaternion>(targetValue);
                Quaternion quaternionValue = ConvertTo<Quaternion>(value);

                Vector3 targetVector3Value = targetQuaternionValue.eulerAngles;
                Vector3 vector3Value = quaternionValue.eulerAngles;

                return (targetVector3Value - vector3Value).magnitude / 360;
            }

            // Not valid Type
            else
            {
                Debug.LogError($"Transition does not support {typeof(T).Name} type");
                return default;
            }
        }

        #endregion

    }
}