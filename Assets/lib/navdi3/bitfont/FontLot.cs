namespace navdi3.bitfont
{

    using UnityEngine;
    using System.Collections;

    public class FontLot : SpriteLot
    {
        public string alphabet;
        public int charWidth;
        public int lineHeight;
        
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
    }

}