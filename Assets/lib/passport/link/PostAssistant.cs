namespace passport.link {

	using Barebones.Networking;

	using passport.crunch;

	public class PostAssistant<ACTION,REPLY> where REPLY:struct {
		IIncommingMessage message;
		public readonly ACTION action;
		public REPLY reply;
		public IPeer Peer {get{return message.Peer;}}
		public bool Done {get;private set;}
		public bool Delayed {get;private set;}
		public PostAssistant(IIncommingMessage message) {
			this.message = message;
			try {
				this.action = Capn.Decrunchatize<ACTION>(message.AsBytes());
			} catch (System.InvalidCastException ex) {
				Dj.Errorf("Still gonna crash, but just a heads up: expecting "+typeof(ACTION).ToString()+", not getting it.");
				throw ex;
			}
			this.reply = new REPLY();
			this.Done = false;
		}
		public void Reply() {
			if (Done) throw Dj.Crash("Can only REPLY or REJECT one time on a PostAssistant");
			message.Respond(Capn.Crunchatize(reply), ResponseStatus.Success);
			Done = true;
		}
		public void Reject(ResponseStatus status) {
			if (Done) throw Dj.Crash("Can only REPLY or REJECT one time on a PostAssistant");
			if (status == ResponseStatus.Success) throw Dj.Crashf("{0} is not a valid REJECTION status", status);
			message.Respond(status);
			Done = true;
		}
		public void Pending() {
			Delayed = true;
		}
	}

    [System.Serializable]
    public struct OK { }
}