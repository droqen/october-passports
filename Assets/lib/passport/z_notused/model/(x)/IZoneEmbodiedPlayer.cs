namespace passport.model {
	public interface IZoneEmbodiedPlayer : playsessions.IPlayerPage {
		bool inzone {get;}
		int zoneid {get;}
	}
}