namespace navdi3
{

    using UnityEngine;
    using System.Collections;

    public class SpriteLot : MonoBehaviour
    {
        public Sprite[] sprites;

        public Sprite this[int index]
        {
            get
            {
                return sprites[index];
            }
        }
        public int Length
        {
            get { return sprites.Length; }
        }

    }

    }