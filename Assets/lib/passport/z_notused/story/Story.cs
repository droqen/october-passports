namespace passport.story {
	using anysub;
	using System.Collections.Generic;
	abstract public class Story {
		abstract public int id {get;}
		abstract public void Publish<T>(short subOp, T issue);
		abstract public void SpawnNewDelta(IDelta delta);
		abstract public void ListenDelta(IDelta delta);
		abstract public bool ApplyNextPendingDelta();
	}
	public class Story<P> : Story where P : IPage {
		override public int id {get{return firstPage.id;}}
		public Storyteller<P> storyteller {get;set;}
		public Story(StoryOptions options, P initialPage) {
			this.options = options;
			// this.subscriptions = new HashSet<Storyteller<P>.IDeltaCallback>();

			if (this.options.maxFutureDeltas == 0) this.options.maxFutureDeltas = 100; // default value
			this.currentPage = initialPage; this.currentPage.SetStory(this);
			this.firstPage = crunch.Capn.Copy<P>(this.currentPage); this.firstPage.SetStory(this);
			this.currentPageNumber = this.currentPage.pagenumber;
			this.nextPageNumber = this.GetNextPageNumber(this.currentPageNumber);

			if (options.storePastDeltas) pastDeltas = new Dictionary<int, IDelta>();
			if (options.deltaListener) futureDeltas = new Dictionary<int, IDelta>();
		}
		public P GetPage() {
			return this.currentPage;
		}
	////base Story
		override public void Publish<T>(short subOp, T value) {
			if (this.storyteller != null) this.storyteller.Publish<T>(subOp, value);
			else Dj.Warn("This Story's teller is null");
		}
		override public void SpawnNewDelta(IDelta delta) {
			if (!options.deltaSpawner) throw Dj.Crash("Can't SpawnNewDelta in a Story that isn't a deltaSpawner");
			delta.storyid = this.id;
			delta.pagenumber = this.nextPageNumber;
			ApplyValidDelta(delta); // just directly apply it
		}
		override public void ListenDelta(IDelta delta) {
			if (!options.deltaListener) throw Dj.Crash("Can't ListenDelta in a Story that isn't a deltaListener");
			if (delta.storyid == this.id) AddFutureDelta(delta);
			else tossedDeltaCount++; // bad storyid
		}
		override public bool ApplyNextPendingDelta() {
			bool applied = false;
			if (futureDeltas.ContainsKey(nextPageNumber)) {
				IDelta delta = futureDeltas[nextPageNumber];
				futureDeltas.Remove(nextPageNumber);
				if (options.storePastDeltas) pastDeltas[nextPageNumber] = delta;
				ApplyValidDelta(delta);
				applied = true;
				tossedDeltaCount = 0;
				CheckMissingNextDelta();
			}
			return applied; // nothing to apply
		}
		public void ApplyAllPendingDeltas(int max=100) {
			for (int i = 0; i <= max && ApplyNextPendingDelta(); i++) {
				if (i == max) Dj.Error("ApplyAllPendingDeltas reached max iterations of "+max+".\nTERMINATING ApplyAllPendingDeltas.");
			}
		}

		public bool DeltaListenerNeedsResync() {
			if (options.deltaListener) {
				if (missingNextDelta) {
					return UnityEngine.Time.time - missingNextDeltaStartTime > 1.0F; // a second seems long enough.
				} else {
					return tossedDeltaCount > 4;
				}
			} else {
				throw Dj.Crash("DeltaListenerNeedsResync can't be called on this story - not a listener");
			}
		}

	////internal
		readonly StoryOptions options;
		// readonly HashSet<Storyteller<P>.IDeltaCallback> subscriptions;
		readonly P firstPage; P currentPage;
		int currentPageNumber, nextPageNumber;
		byte tossedDeltaCount;
		bool missingNextDelta;
		float missingNextDeltaStartTime;
		void CheckMissingNextDelta() {
			if (missingNextDelta) {
				if (futureDeltas.Count == 0) {
					missingNextDelta = false;
				}
			} else {
				if (futureDeltas.Count > 0 && !futureDeltas.ContainsKey(nextPageNumber)) {
					missingNextDelta = true;
					missingNextDeltaStartTime = UnityEngine.Time.time;
				}
			}
		}
		int GetNextPageNumber(int pagenumber) { if (pagenumber == int.MaxValue) return 1; else return pagenumber + 1; }
		int GetDeltasUntil(int pagenumber) {
			int deltasUntil = pagenumber - currentPageNumber;
			if (deltasUntil < 0) return deltasUntil + int.MaxValue; 
			return deltasUntil;
		}
		Dictionary<int, IDelta> pastDeltas;
		Dictionary<int, IDelta> futureDeltas;

		bool AddFutureDelta(IDelta delta) {
			bool nearFuture = GetDeltasUntil(delta.pagenumber) <= options.maxFutureDeltas;
			if (nearFuture) {
				futureDeltas[delta.pagenumber] = delta;
				CheckMissingNextDelta();
				return false;
			} else {
				// too far in the future
				if (tossedDeltaCount<byte.MaxValue) tossedDeltaCount ++;
				return false;
			}
		}
		void ApplyValidDelta(IDelta delta) {
			this.currentPageNumber = delta.pagenumber;
			this.nextPageNumber = GetNextPageNumber(this.currentPageNumber);
			this.currentPage.StoryApplyDelta(delta);
			if (storyteller!=null&&storyteller.OnAnyDelta!=null) storyteller.OnAnyDelta(this, delta);
		}
	}
}