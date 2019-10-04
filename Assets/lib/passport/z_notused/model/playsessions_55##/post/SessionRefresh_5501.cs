namespace passport.model.playsessions.post {
	[System.Serializable]
	public struct SessionRefresh {
		public const short op = 5501;
	}
	[System.Serializable]
	public struct SessionRefresh_Reply {
		public bool okay;
		public SessionPage session;
	}
}