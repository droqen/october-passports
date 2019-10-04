namespace passport.model.playsessions {

	using Barebones.Networking;
	using System.Collections.Generic;

	using passport.anysub;
	using passport.link;
	using passport.story;

	public partial class SModelPlaysessions<PlayerPage> : SModelBase, IPlayerToSession where PlayerPage : IPlayerPage {

	////properties
		public IUsernameDb userdb = new CheapUsernameDb();
		public ISessionKeyGen skeygen = new CheapSessionKeyGen();
		public IPlayerSpawner pspawner = new CheapBasePlayerSpawner();

		Storyteller<SessionPage> sessionsTeller;
		Storyteller<PlayerPage> playersTeller;
		Dictionary<int,int> peerid_to_playerid;

	////events
		System.Action<IPeer> OnLogin;
		System.Action<int> OnLogout;

	////constructor

		public SModelPlaysessions(ServersideLink link, System.Action<IPeer> OnLogin, System.Action<int> OnLogout) : base(link) {
			this.OnLogin = OnLogin;
			this.OnLogout = OnLogout;

			this.sessionsTeller = new Storyteller<SessionPage>(null);
			this.playersTeller = new Storyteller<PlayerPage>(BroadcastPlayerDelta);

			this.peerid_to_playerid = new Dictionary<int, int>();
			// this.storyteller.AddUniqueColumn<string>(COL_USERNAME, page=>{return page.username;});

			this.sessionsTeller.Subscribe<SessionPage>( SessionPage.SUBOP_DiscardPeerId, session => {
				if (peerid_to_playerid.ContainsKey(session.ServerOnly_peerid) && peerid_to_playerid[session.ServerOnly_peerid] == session.playerid)
					peerid_to_playerid.Remove(session.ServerOnly_peerid);
			} );
			this.sessionsTeller.Subscribe<SessionPage>( SessionPage.SUBOP_MyPeerIdChanged, session => {
				peerid_to_playerid[session.ServerOnly_peerid] = session.playerid;
			} );

			this.LINKOPS_SetPostHandlers();
			this.link.OnPeerDisconnect = (peerid=>{this.BANG_LogoutPeer(peerid);});
		}
		
		public Story<SessionPage> GetSession(int playerid) {
			return sessionsTeller.GetStory(playerid);
		}
		public Story<SessionPage> GetSession(IPeer peer) {
			int playerid = 0;
			if (peerid_to_playerid.TryGetValue(peer.Id, out playerid)) return GetSession(playerid);
			else return null;
		}
		public Story<PlayerPage> GetPlayer(int playerid) {
			var player = playersTeller.GetStory(playerid);
			return player;
		}



		public void BroadcastPlayerDelta(Story<PlayerPage> player, IDelta delta) {
			var session = GetSession(player.id);
			if (session != null) {
				link.Post(
					session.GetPage().ServerOnly_peerid,
					bc.PlayerDelta.op,
					new bc.PlayerDelta{idelta=delta}
				);
			} else {
				Dj.Warnf("Player#{0} has no valid session",player.id);
			}
		}
	}

	class CheapUsernameDb : IUsernameDb {
		int nextuserid = 1;
		Dictionary<string, int> cheapUsers = new Dictionary<string, int>();
		public bool TryGetUsernameDetails(ref string username, out int userid, out string hashpass) {
			username = username.ToLower();
			if (!cheapUsers.TryGetValue(username, out userid)) {
				userid = nextuserid++;
				cheapUsers.Add(username, userid);
			}
			hashpass = "*";
			return true;
		}
	}

	class CheapSessionKeyGen : ISessionKeyGen {
		public string NewSessionKey() {
			string skey = "";
			for (int i = 0; i < 16; i++) skey += (char)UnityEngine.Random.Range('!','z'+1);
			return skey;
		}
	}

	class CheapBasePlayerSpawner : IPlayerSpawner {
		public IPlayerPage SpawnPlayer(SessionPage session) {
			return new BasePlayerPage(session.id, session.username);
		}
	}

}