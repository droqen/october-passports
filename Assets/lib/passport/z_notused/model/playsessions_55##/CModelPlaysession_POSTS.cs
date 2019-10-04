namespace passport.model.playsessions {

	using Barebones.Networking;

	using passport.story;

	public partial class CModelPlaysession<PlayerPage> {
		public void POST_ConnectAndLogin(string username, System.Action OnFail = null) {
			link.AttemptConnection(success=>{
				if (success) {
					link.Post(post.SessionLogin.op,
					new post.SessionLogin{
						username=username,
						hashpass="*"
					},
					(post.SessionLogin_Reply reply)=>{
						if (reply.okay) {
							ServersideSetSession(new Story<SessionPage>(StoryOptions.Listener, reply.sessionPage));
						} else {
							if (OnFail != null) OnFail();
						}
					},
					(failure)=>{
						if (OnFail != null) OnFail();
					});
				}
			});
		}
	}

}