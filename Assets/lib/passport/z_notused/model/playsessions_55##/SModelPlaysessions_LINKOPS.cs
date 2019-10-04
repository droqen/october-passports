namespace passport.model.playsessions {

	using Barebones.Networking;

	using passport.crunch;
	using passport.story;

	public partial class SModelPlaysessions<PlayerPage> : SModelBase where PlayerPage : IPlayerPage {
		public void LINKOPS_SetPostHandlers() {
			this.link.SetPostHandler(post.SessionLogin.op, message=>{
				var login = Capn.Decrunchatize<post.SessionLogin>(message.AsBytes());
				var reply = new post.SessionLogin_Reply();

				if (login.playerid > 0 && login.sessionkey != default(string)) {

					// try login via userid x sessionkey
					var session = sessionsTeller.GetStory(login.playerid);
					if (session != null && session.GetPage().sessionkey == login.sessionkey && !session.GetPage().ServerOnly_IsExpired()) {
						Story<PlayerPage> player;
						if (BANG_Login(session, message.Peer)) {
							reply.okay = true;
							reply.sessionPage = session.GetPage();
						} else reply.bad_credentials = true; // idk
					} else {
						reply.bad_credentials = true;
					}

				} else if (login.username.Length > 0 && login.hashpass.Length > 0) {

					// try login via username x hashpass
					int dbuserid;
					string dbhashpass;
					if (userdb.TryGetUsernameDetails(ref login.username, out dbuserid, out dbhashpass) && (login.hashpass == dbhashpass || dbhashpass == "*")) {
						Story<SessionPage> session;
						Story<PlayerPage> player;
						if (BANG_Login(dbuserid, login.username, message.Peer, out session)) {
							reply.okay = true;
							reply.sessionPage = session.GetPage();
						} else {
							// login failed
						}
					} else {
						// user does not exist. login failed.
					}

					if (!reply.okay) reply.bad_credentials = true;

				} else {

					reply.bad_params = true;

				}

				message.Respond(Capn.Crunchatize(reply), ResponseStatus.Success);
			});

			this.link.SetPostHandler(post.SessionLogout.op, message=>{
				// var action = Capn.Decrunchatize<post.SessionLogout>(message.AsBytes());
				var reply = new post.SessionLogout_Reply();

				reply.okay = BANG_LogoutPeer(message.Peer);

				message.Respond(Capn.Crunchatize(reply), ResponseStatus.Success);
			});

			this.link.SetPostHandler(post.SessionRefresh.op, message=>{
				// var action = Capn.Decrunchatize<post.SessionRefresh>(message.AsBytes());
				var reply = new post.SessionRefresh_Reply();

				var story = GetSession(message.Peer);
				if (story != null) {
					reply.okay = true;
					reply.session = story.GetPage();
				}
				
				message.Respond(Capn.Crunchatize(reply), ResponseStatus.Success);
			});
		}
	}

}