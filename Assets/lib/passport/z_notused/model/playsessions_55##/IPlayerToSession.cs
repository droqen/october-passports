namespace passport.model.playsessions {
	using passport.story;
	public interface IPlayerToSession {
		Story<SessionPage> GetSession(int playerid);
	}
}