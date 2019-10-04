using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace navdi3
{

    public class ChildTweener : MonoBehaviour
    {
        public TweenState startState;
        public TweenState endState;
        public float startTime;
        public float endTime;
        public bool tweening = false;
        void Update()
        {
            if (tweening)
            {
                var progress = Mathf.Clamp01(Mathf.InverseLerp(startTime, endTime, Time.time));
                TweenState.Lerp(startState, endState, progress).Apply(transform.GetChild(0));
                if (progress >= 1) tweening = false; // stop tweening.
            }
        }
        public void SetupTween(TweenState start, TweenState end, float duration, float delay = 0.0f)
        {
            if (duration < float.Epsilon) throw new System.Exception("SetupTween must have a positive duration parameter");

            startState = start; endState = end;
            startTime = Time.time + delay; endTime = startTime + duration;
            tweening = true;
        }
    }

    public struct TweenState
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public Color spriteRendererColor;

        public TweenState(Transform child)
        {
            SpriteRenderer spriter = child.GetComponent<SpriteRenderer>();
            this.localPosition = child.localPosition;
            this.localRotation = child.localRotation;
            this.localScale = child.localScale;
            this.spriteRendererColor = (spriter != null) ? (spriter.color) : (Color.clear);
        }
        public static TweenState Lerp(TweenState a, TweenState b, float t)
        {
            return new TweenState
            {
                localPosition = Vector3.Lerp(a.localPosition, b.localPosition, t),
                localRotation = Quaternion.Lerp(a.localRotation, b.localRotation, t),
                localScale = Vector3.Lerp(a.localScale, b.localScale, t),
                spriteRendererColor = Color.Lerp(a.spriteRendererColor, b.spriteRendererColor, t),
            };
        }
        public void Apply(Transform child)
        {
            child.localPosition = localPosition;
            child.localRotation = localRotation;
            child.localScale = localScale;

            SpriteRenderer spriter = child.GetComponent<SpriteRenderer>();
            if (spriter) spriter.color = spriteRendererColor;
        }
    }
}