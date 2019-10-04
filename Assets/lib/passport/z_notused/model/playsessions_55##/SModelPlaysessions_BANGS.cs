namespace passport.model.playsessions {

	using Barebones.Networking;

	using passport.story;

	public partial class SModelPlaysessions<PlayerPage> : SModelBase where PlayerPage : IPlayerPage {
		public bool BANG_Login(Story<SessionPage> session, IPeer peer) {
			int playerid = 0;
			if (peerid_to_playerid.TryGetValue(peer.Id, out playerid)) {
				// this peer is already connected to a user. Logout first!
				return false;
			} else {
				BANG_Logout(session.id); // logout of old session
				peerid_to_playerid[peer.Id] = session.id;
				if (sessionsTeller.GetStory(session.id) == null) sessionsTeller.AddStory(session);
				session.GetPage().SetPeer(session, peer.Id, skeygen.NewSessionKey());

				var player = playersTeller.GetStory(session.id);
				if (player == null) player = new Story<PlayerPage>(StoryOptions.Spawner, (PlayerPage)pspawner.SpawnPlayer(session.GetPage()));
				playersTeller.AddStory(player);

				OnLogin(peer);

				return true;
			}
		}

		public bool BANG_Login(int playerid, string username, IPeer peer, out Story<SessionPage> session) {
			session = sessionsTeller.GetStory(playerid);
			if (session == null) {
				session = new Story<SessionPage>(new StoryOptions{deltaSpawner=true}, new SessionPage(playerid, username));
			}
			if (BANG_Login(session, peer)) {
				// storyteller.AddStory(story);
				return true;
			} else {
				session = null;
				return false;
			}
		}

		public bool BANG_Logout(int playerid) {
			var session = GetSession(playerid);
			if (session != null) {
				session.GetPage().ServerOnly_Activity();

				OnLogout(session.id);
				return true; // ok, i logged u out
			} else {
				return false; // nobody to log out
			}
		}
		public bool BANG_LogoutPeer(int peerid) {
			int playerid = 0;
			if (peerid_to_playerid.TryGetValue(peerid, out playerid)) return BANG_Logout(playerid);
			else return false;
		}
		public bool BANG_LogoutPeer(IPeer peer) {
			return BANG_LogoutPeer(peer.Id);
		}
	}

}