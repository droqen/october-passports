namespace passport.sundial {

	[System.Serializable]
	public class TweenInt : BaseTweenValue<int> {
		public readonly int startingValue, diff;
		public int endingValue { get { return startingValue + diff; } }
		// readonly int startingValue, endingValue, diff;
		// startingValue = 0,
		// endingValue = 2,
		// startingTime = 0,
		// endingTime = 1
		// i guess this means i want (int)(tween*diff)
		public TweenInt(int startingValue, int endingValue, float startingTime, float endingTime) : base(startingTime, endingTime) {
			this.startingValue = startingValue;
			// this.endingValue = endingValue;
			this.diff = endingValue - startingValue;
		}
		sealed override public int GetValueAtTween(float tween) {
			if (diff >= 0) return startingValue + (int)(tween * diff); // if diff is 3 (1 -> 4), tween*diff @ 0.999 = 2.997, results in value just below max. perfect!
			else return startingValue - (int)(-tween * diff);
		}
	}
}