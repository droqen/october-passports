namespace passport.sundial {
	public interface ITweenValue<T> {
		T CurrentValue {get;}
		bool NotStartedYet {get;}
		bool Done {get;}
	}
}