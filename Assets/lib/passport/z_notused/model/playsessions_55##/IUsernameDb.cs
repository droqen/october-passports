namespace passport.model.playsessions {
	public interface IUsernameDb {
		bool TryGetUsernameDetails(ref string username, out int userid, out string hashpass);
	}
}