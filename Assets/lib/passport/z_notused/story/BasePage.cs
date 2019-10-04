namespace passport.story {

	using anysub;
	using System.Collections.Generic;

	[System.Serializable]
	abstract public class BasePage<D> : IPage where D : IDelta {

		abstract public bool ApplyDelta(D delta);
		public void Publish<T>(short subOp, T value) {
			if (this.story != null) this.story.Publish<T>(subOp, value);
			else Dj.Warn("This Page's story is null");
		}

	////IPage
		public int pagenumber {get;set;}
		public int id {get;private set;}
		public BasePage(int id) {
			this.id = id;
		}
		public void SetStory(Story story) {
			this.story = story;
		}
		public void StoryApplyDelta(IDelta delta) {
			if (delta is D) {
				this.pagenumber = delta.pagenumber;
				if (this.ApplyDelta((D)delta)) {
					// ok, handled
				} else {
					Dj.Errorf("Page {0}.ApplyDelta failed on delta {1}\n-- return false",this,delta);
				}
			} else {
				Dj.Crashf("BasePage expected delta of type {0}\n-- but got bad delta {1}",typeof(D),delta);
			}
		}
		[System.NonSerialized] Story story;
		protected void BANG(D delta) {
			if (story==null) Dj.Error("BasePage.BANG but story==null");
			else story.SpawnNewDelta(delta);
		}

	}
}