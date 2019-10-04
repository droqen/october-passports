namespace passport.model.playsessions {

	using UnityEngine;

	using passport.story;

	[System.Serializable]
	public class SessionPage : BasePage<Delta> {
		public bool connected;
		public int playerid {get{return id;}}
		public string username;
		public string sessionkey;
		public SessionPage(int id, string username) : base(id) {
			this.username = username; // doesn't change
			this.ServerOnly_peerid = -1;
			this.ServerOnly_lastActivityTime = Time.time;
			this.sessionkey = "";
		}
		override public bool ApplyDelta(Delta delta) {
			// switch(delta.op) {
			// 	case 550: this.ServerOnly_peerid = (int)delta.data[0]; this.sessionkey = (string)delta.data[1]; break;
			// }
			return false;
		}

		public void SetPeer(Story<SessionPage> story, int peerid, string sessionkey) {

			ServerOnly_Activity();

			if (this.ServerOnly_peerid != peerid) {
				Publish(SUBOP_DiscardPeerId, this);
				this.ServerOnly_peerid = peerid;
				Publish(SUBOP_MyPeerIdChanged, this);
			}

			this.sessionkey = sessionkey;

			// story.SpawnNewDelta(new Delta(550, peerid, sessionkey));
		}

		[System.NonSerialized] public int ServerOnly_peerid;
		[System.NonSerialized] public float ServerOnly_lastActivityTime;

		public void ServerOnly_Activity() {
			ServerOnly_lastActivityTime = Time.time;
		}
		public bool ServerOnly_IsExpired() {
			return (Time.time > ServerOnly_lastActivityTime + 1500); // expires after 1500 seconds: a bit less than half an hour.
		}

		public const int SUBOP_DiscardPeerId = 555;
		public const int SUBOP_MyPeerIdChanged = 666;



	}
}