namespace passport.story {
	public interface IPage {
		int pagenumber {get;set;}
		int id {get;}
		void SetStory(Story story);
		void StoryApplyDelta(IDelta delta);
	}
}