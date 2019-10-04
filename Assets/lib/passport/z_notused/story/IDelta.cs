namespace passport.story {
	public interface IDelta {
		bool valid {get;} bool invalid {get;}
		int storyid {get;set;}
		int pagenumber {get;set;}
	}
}