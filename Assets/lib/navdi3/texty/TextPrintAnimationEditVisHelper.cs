using UnityEngine;
using TMPro;

namespace navdi3.texty
{
    [RequireComponent(typeof(TextPrintAnimation))]
    [RequireComponent(typeof(TextMeshPro))]

    [ExecuteInEditMode]
    public class TextPrintAnimationEditVisHelper : MonoBehaviour
    {
        private void Update()
        {
            if (Application.isPlaying) { enabled = false; return; }
            GetComponent<TextMeshPro>().text = GetComponent<TextPrintAnimation>().text;
        }
    }
}