
namespace passport.model.playsessions {

	using Barebones.Networking;
	using System.Collections.Generic;

	using passport.anysub;
	using passport.crunch;
	using passport.link;
	using passport.story;

	public partial class CModelPlaysession<PlayerPage> : CModelBase where PlayerPage : IPlayerPage {

	////properties
		Story<SessionPage> mySession;
		// Storyteller<SessionPage> mySessionTeller;
		Story<PlayerPage> myPlayer;
		System.Action<Story<SessionPage>> RequestNewSessionPages;
		System.Action<Story<PlayerPage>> ShowNewPlayer;

	////constructor

		public CModelPlaysession(ClientsideLink link, System.Action<Story<SessionPage>> RequestNewSessionPages, System.Action<Story<PlayerPage>> ShowNewPlayer) : base(link) {
			this.RequestNewSessionPages = RequestNewSessionPages;
			this.ShowNewPlayer = ShowNewPlayer;
			this.link.OnDisconnected += () => {
				ServersideSetPlayer(null);
				// TODO: automatically handle reconnection attempts and session dropping.
			};
			this.link.SetPostHandler(
				bc.PlayerDelta.op,
				message=>{
					var action=Capn.Decrunchatize<bc.PlayerDelta>(message.AsBytes());
					if (this.myPlayer != null) {
						this.myPlayer.ListenDelta(action.idelta);
						this.myPlayer.ApplyAllPendingDeltas();
					}
				}
			);
		}

		public bool LoggedIn { get { return myPlayer != null; } }
		public PlayerPage Player { get { return LoggedIn? myPlayer.GetPage() : default(PlayerPage); } }

		void ServersideSetSession(Story<SessionPage> session) {
			this.mySession = session;
			RequestNewSessionPages(this.mySession);
		}

		public void ServersideSetPlayer(Story<PlayerPage> player) {
			this.myPlayer = player;
			ShowNewPlayer(this.myPlayer);
		}
		public void ServersideSetPlayerPage(PlayerPage playerPage) {
			ServersideSetPlayer((playerPage == null) ? null : new Story<PlayerPage>(StoryOptions.Listener, playerPage));
		}

		// public event System.Action<Story<SessionPage>> OnSessionSet;
		// public event System.Action<Story<PlayerPage>> OnPlayerSet;

	}

}