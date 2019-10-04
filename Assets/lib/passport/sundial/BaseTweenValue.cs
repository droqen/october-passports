namespace passport.sundial {
	[System.Serializable]
	abstract public class BaseTweenValue<T> : ITweenValue<T> {
		public bool showEndingValueBeforeStartTime = true;
		float startTime {get;set;} float endTime {get;set;} // flatten
		readonly float durationRecip;
		public BaseTweenValue(float startTime, float endTime) {
			this.startTime = startTime;
			this.endTime = endTime;
			if (endTime > startTime) {
				this.durationRecip = 1 / (endTime - startTime);
			}
		}
		public T CurrentValue {get{
			float sun = Sun.dial.GetTime();
			if (endTime<=0) {
				return GetValueAtTween(1f); // just always get flattened 'ending' value
			} else if (IsDoneAtSun(sun)) {
				endTime=0; Flatten();
				return GetValueAtTween(1f);
			} else if (IsNotStartedAtSun(sun)) {
				if (showEndingValueBeforeStartTime) return GetValueAtTween(1f);
				else return GetValueAtTween(0f);
			} else {
				float tween = (sun-startTime) * durationRecip;
				return GetValueAtTween(tween);
			}
		}}
		public bool NotStartedYet {get{
			return IsNotStartedAtSun(Sun.dial.GetTime());
		}}
		public bool Done {get{
			return IsDoneAtSun(Sun.dial.GetTime());
		}}

		virtual public void Flatten() {
			this.startTime = 0;
			this.endTime = 0;
		}

		bool IsNotStartedAtSun(float sun) { return sun < startTime; }
		bool IsDoneAtSun(float sun) { return sun >= endTime; }

		abstract public T GetValueAtTween(float tween);

	}
}