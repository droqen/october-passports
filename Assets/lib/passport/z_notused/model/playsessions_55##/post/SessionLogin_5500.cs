namespace passport.model.playsessions.post {
	[System.Serializable]
	public struct SessionLogin {
		public const short op = 5500;
		public string username;
		public string hashpass;
		public int playerid;
		public string sessionkey;
	}
	[System.Serializable]
	public struct SessionLogin_Reply {
		public bool okay;
		public SessionPage sessionPage;
		public bool bad_credentials;
		public bool bad_params;
	}
}