namespace passport.sundial {
	public interface ISundial {
		float GetTime();
		void SetTime(float myTime, float? realTime = null, bool replaceAllSamples = false);
	}
}