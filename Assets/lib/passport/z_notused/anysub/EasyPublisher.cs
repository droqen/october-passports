namespace passport.anysub {
	using System.Collections.Generic;
	public class EasyPublisher : ISubscribable
	{
		public EasyPublisher() {
			this.subscriptions = new Dictionary<short, Sub>();
		}
		public void Subscribe<T>(short subOp, System.Action<T> subAction) {
			this.subscriptions.Add(subOp, new Sub<T>(subAction));
		}
		public void Publish<T>(short subOp, T value) {
			if (this.subscriptions.ContainsKey(subOp)) {
				this.subscriptions[subOp].Invoke(value);
			} else {
				Dj.Warnf("NO SUBSCRIPTION WARNING! subOp {0}\nPublished {0}:{1}({2})", subOp, value, typeof(T));
			}
		}
		Dictionary<short,Sub> subscriptions;
	}

	abstract class Sub {
		abstract public void Invoke(object value);
	}
	class Sub<FIXTYPE> : Sub {

	////base Sub
		override public void Invoke(object value) {
			Invoke((FIXTYPE)value);
		}

		public Sub(System.Action<FIXTYPE> action) {
			this.action = action;
		}
		public void Invoke(FIXTYPE value) {
			this.action(value);
		}
		System.Action<FIXTYPE> action;
	}

}