namespace passport.model {
	using passport.link;
	using passport.story;
	abstract public class CModelBase : ModelBase {
		public CModelBase(ClientsideLink link) {
			this.link = link;
		}
		protected ClientsideLink link;
	}
}