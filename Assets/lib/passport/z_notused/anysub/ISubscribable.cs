namespace passport.anysub {
	public interface ISubscribable
	{
		void Subscribe<T>(short subOp, System.Action<T> ReadIssue);
		void Publish<T>(short subOp, T issue);
	}
}