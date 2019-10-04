namespace passport.sundial {
	[System.Serializable]
	public class TweenArray<T> : BaseTweenValue<T> {

		readonly T[] array;
		// readonly int startingValue, endingValue, diff;
		// startingValue = 0,
		// endingValue = 2,
		// startingTime = 0,
		// endingTime = 1
		// i guess this means i want (int)(tween*diff)
		public TweenArray(T[] array, float startingTime, float endingTime) : base(startingTime, endingTime) {
			this.array = array;
		}
		override public T GetValueAtTween(float tween) {
			int index = (int)(tween * (array.Length-1));
			return array[index];
		}
	}
}