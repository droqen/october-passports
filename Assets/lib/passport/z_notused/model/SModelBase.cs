namespace passport.model {
	using passport.link;
	using passport.story;
	abstract public class SModelBase : ModelBase {
		public SModelBase(ServersideLink link) {
			this.link = link;
		}
		protected ServersideLink link;
	}
}