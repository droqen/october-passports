namespace passport.story {
	
	[System.Serializable]
	public struct Delta : IDelta {
	////public
		public bool valid { get { return op > 0; } }
		public bool invalid { get { return op <= 0; } }
		public short op;
		public object[] data;
	////constructor
		public Delta(short op, params object[] data) {
			this.storyid = -1; // pleas remember to assign
			this.pagenumber = 0; // default value anyway? but just in case.
			this.op = op;
			this.data = data;
		}
	////IDelta
		public int storyid{get;set;}
		public int pagenumber{get;set;}
	}
}