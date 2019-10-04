namespace passport.model.playsessions.post {
	[System.Serializable]
	public struct SessionLogout {
		public const short op = 5502;
	}
	[System.Serializable]
	public struct SessionLogout_Reply {
		public bool okay;
	}
}