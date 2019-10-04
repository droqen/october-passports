//namespace navdi3.bitfont {
//	using navdi3;
//	using System.Collections.Generic;
//	using UnityEngine;
//	public class Text : BankEnts<Char> {
//		public twin rootPosition {
//			get { return _rootPosition; }
//			set { _rootPosition = value; RefreshPositions(); }
//		}
//		public string text {
//			get { return _text; }
//			set { _text = value; if (active) Refresh(); }
//		}
//		public Color colour {
//			get { return _colour; }
//			set { _colour = value; if (active) foreach(var c in chars) c.spriter.color = value; }
//		}
//		public Text(Bank charBank, twin rootPosition, string startingText = "", bool centered = false, int sortingOrder = 0, Color colour = default(Color)) {
//			this.chars = new List<Char>();
			
//			this.charBank = charBank;

//			this.centered = centered;
//			this.sortingOrder = sortingOrder;
//			this.colour = colour==default(Color)?Color.black:colour;
			
//			this.rootPosition = rootPosition;
//			this.text = startingText;
//		}
//	////BankableEntity
//		override public void Activated() {
//			base.Activated();
//			Refresh();
//		}
//	////BankEnts<Char>
//		override public bool Add(Char c) {
//			if (base.Add(c)) {
//				c.spriter.sortingOrder = this.sortingOrder;
//				c.spriter.color = this.colour;
//				chars.Add(c); return true;
//			} else return false;
//		}
//		override public bool Remove(Char c) {
//			if (base.Remove(c)) {
//				chars.Remove(c); return true;
//			} else return false;
//		}
//	////REfreshPositions - what's this for?
//		virtual protected void RefreshPositions(int start = 0) {
//			if (chars.Count==0) return;
//			if (centered) {
//				twin shiftedPosition = rootPosition + twin.left * (text.Length * chars[0].font.charWidth / 2);
//				for (int i = 0; i < text.Length; i++) {
//					chars[i].position = shiftedPosition + new twin(i*chars[i].font.charWidth,0);
//				}
//			} else {
//				for (int i = start; i < text.Length; i++) {
//					chars[i].position = rootPosition + new twin(i*chars[i].font.charWidth,0);
//				}
//			}
//		}
//	////Internal Shit
//		void Refresh() {
//			SetBodsCount(text.Length);
//			for (int i = 0; i < text.Length; i++) {
//				chars[i].Show(text[i]);
//			}
//		}
//		void SetBodsCount(int count) {
//			if (count < 0) count = 0;
//			int delta = count - chars.Count;
//			if (delta > 0) { // need more
//				int previousBodCount = chars.Count;
//				for (int i = 0; i < delta; i++) {
//					Add(charBank.Spawn<Char>().Setup());
//				}
//				RefreshPositions(previousBodCount);
//			}
//			if (delta < 0) { // need fewer
//				int bodRemoveIndex = chars.Count - 1;
//				for (int i = 0; i > delta; i--) {
//					chars[bodRemoveIndex].Sleep();
//					bodRemoveIndex--;
//				}
//			}
//			// if (delta == 0) {} // perfect, don't do anything
//		}
//	////Internal properties
//		Bank charBank;
//		List<Char> chars;
//		twin _rootPosition;
//		Color _colour;
//		string _text;
//		bool centered;
//		int sortingOrder;
//	}
//}