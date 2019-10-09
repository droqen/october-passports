namespace navdi3.bitfont
{

    using UnityEngine;
    using System.Collections;

    [RequireComponent(typeof(BankLot))]

    public class FontLot : SpriteLot
    {
        public string alphabet;
        public int charWidth;
        public int lineHeight;

        BankLot banks { get { return GetComponent<BankLot>(); } }
        
		public Sprite GetCharSprite(char c) {
			int index = alphabet.IndexOf(c);
			if (index >= 0) {
				if (index < this.Length) {
					return this[index];
				} else {
					Dj.Error("Font "+name+" index >= Length. alphabet/sprs mismatch?");
				}
			}
			return null;
		}

        public GameObject SpawnChar(char c, Transform parent, Vector3 position)
        {
            var sprite = GetCharSprite(c);
            var cent = banks["char"].Spawn(parent, position);
            cent.GetComponent<SpriteRenderer>().sprite = sprite;
            return cent;
        }
    }

}