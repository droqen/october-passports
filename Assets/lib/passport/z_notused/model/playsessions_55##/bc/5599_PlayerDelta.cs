namespace passport.model.playsessions.bc {

	using passport.story;

	[System.Serializable]
	public struct PlayerDelta {
		public const short op = 5599;
		public IDelta idelta;
	}
}