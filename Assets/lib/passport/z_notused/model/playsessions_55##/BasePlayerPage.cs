namespace passport.model.playsessions {

	using System.Collections.Generic;
	using UnityEngine;

	using passport.story;
	using passport.sundial;

	[System.Serializable]
	public class BasePlayerPage : BasePage<Delta>, IPlayerPage {
		public string name {get;private set;}
		public BasePlayerPage(int id, string name) : base(id) {
			if (name.Length <= 1) this.name = name.ToUpper();
			else this.name = name[0].ToString().ToUpper() + name.Substring(1).ToLower();
		}
		override public bool ApplyDelta(Delta delta) {
			switch(delta.op) {
				default: return false;
			}
			return true;
		}
	}
}