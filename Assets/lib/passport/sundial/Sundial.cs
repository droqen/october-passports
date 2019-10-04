namespace passport.sundial {
	using UnityEngine;
	public class Sundial : MonoBehaviour, ISundial {

		void Awake() {
			Sun.dial = this;
			// SetTime( 0 );
		}
		// public Sundial() {
		// 	SetTime( 0 );
		// }

		float offUnityTime;
		float[] offUnityTimeSamples;
		int nextSampleIndex;
		const int sampleCount = 10;

		public float GetTime() {
			return Time.time + offUnityTime;
		}
		public void SetTime(float myTime, float? realTime = null, bool replaceAllSamples = false) {
			if (!realTime.HasValue) realTime = Time.time;
			float off = myTime - realTime.Value;

			if (this.offUnityTimeSamples==null||replaceAllSamples) {
				this.offUnityTimeSamples = new float[sampleCount];
				for(int i = 0; i < sampleCount; i++) this.offUnityTimeSamples[i] = off;
				this.nextSampleIndex = 0;
			} else {
				this.offUnityTimeSamples[nextSampleIndex] = off;
				this.nextSampleIndex = (nextSampleIndex+1)%sampleCount;
			}

			float offsum = 0;
			for(int i = 0; i < sampleCount; i++) offsum += this.offUnityTimeSamples[i];

			this.offUnityTime = offsum / sampleCount; // average of past {sampleCount} samples
		}
	}
}