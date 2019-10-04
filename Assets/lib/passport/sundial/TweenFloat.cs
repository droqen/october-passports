namespace passport.sundial {

	[System.Serializable]
	public class TweenFloat : BaseTweenValue<float> {
		public readonly float startingValue, diff;
		public float endingValue { get { return startingValue + diff; } }
		public TweenFloat(float startingValue, float endingValue, float startingTime, float endingTime) : base(startingTime, endingTime) {
			this.startingValue = startingValue;
			// this.endingValue = endingValue;
			this.diff = endingValue - startingValue;
		}
		sealed override public float GetValueAtTween(float tween) {
			if (diff >= 0) return startingValue + (float)(tween * diff); // if diff is 3 (1 -> 4), tween*diff @ 0.999 = 2.997, results in value just below max. perfect!
			else return startingValue - (float)(-tween * diff);
		}
	}
}