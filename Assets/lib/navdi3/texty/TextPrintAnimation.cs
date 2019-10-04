using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace navdi3.texty
{

    [RequireComponent(typeof(TextPrintAnimationEditVisHelper))]
    [RequireComponent(typeof(TextMeshPro))]

    public class TextPrintAnimation : MonoBehaviour
    {

        [TextArea]
        public string text;
        public float framesPerCharacter = 2;
        public int framesPerShortPause = 5;
        public char[] shortPauseCharacters = { ',', ':', ';', '-', };
        public int framesPerPause = 12;
        public char[] pauseCharacters = { '.', '?', '!', };
        [Header("Runtime variables")]
        public int progress = 0;
        public int pause = 0;

        private TMP_Text m_TextComponent;
        private bool done = true;
        private string renderedText = "";

        private void Awake()
        {
            m_TextComponent = GetComponent<TMP_Text>();
        }

        private void FixedUpdate()
        {
            if (text != renderedText)
            {
                renderedText = text;
                done = false;
            }

            if (pause > 0)
            {
                pause--;
            }
            else if (!done)
            {
                progress++;
                if (progress >= text.Length * framesPerCharacter)
                {
                    done = true;
                    m_TextComponent.text = text;
                }
                else
                {
                    int hideBreakPosition = Mathf.FloorToInt(progress / framesPerCharacter);
                    m_TextComponent.text = text.Substring(0, hideBreakPosition) + "<#00000000>" + text.Substring(hideBreakPosition);
                    if (hideBreakPosition > 0 && text[hideBreakPosition] == ' ') // all pause characters must be followed by a space
                    {
                        foreach (var c in shortPauseCharacters)
                            if (text[hideBreakPosition - 1] == c)
                                pause += framesPerShortPause;
                        foreach (var c in pauseCharacters)
                            if (text[hideBreakPosition - 1] == c)
                                pause += framesPerPause;
                    }
                }
            }
        }
    }
}