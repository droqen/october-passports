namespace passport.story {
	using anysub;
	using System.Collections;
	using System.Collections.Generic;

	abstract public class StorytellerUtils {
		public delegate K ColumnValueGetter<K,P>(P page) where P : IPage;
	}

	abstract public class Storyteller : ISubscribable {
		public Storyteller(ISubscribable sub) {
			this.sub = sub;
		}
		public void Subscribe<T>(short subOp, System.Action<T> ReadIssue) { sub.Subscribe<T>(subOp, ReadIssue); }
		public void Publish<T>(short subOp, T issue) { sub.Publish<T>(subOp, issue); }
		ISubscribable sub;
	}
	public class Storyteller<P> : Storyteller where P : IPage {
		public delegate void IDeltaCallback(Story<P> story, IDelta delta);
		public IDeltaCallback OnAnyDelta;
		public Storyteller(IDeltaCallback OnAnyDelta) : base(new EasyPublisher()) {
			this.OnAnyDelta = OnAnyDelta;
			this.stories = new HashSet<Story<P>>();
			this.columns = new Dictionary<byte, Column<P>>();
			this.idColumn = new Column<int, P>(page=>{return page.id;});
		}
		public void AddUniqueColumn<K>(byte columnId, StorytellerUtils.ColumnValueGetter<K,P> GetColValue) {
			columns.Add(columnId, new Column<K,P>(GetColValue));
		}
		public void AddStory(Story<P> story) {
			try {
				idColumn.AddStory(story);
				foreach(var kvp in columns) kvp.Value.AddStory(story);
			} catch (System.Exception e) { Dj.Errorf("AddStory failed\n{0}",e); return; }
			story.storyteller = this;
			stories.Add(story);
		}
		public void RemoveStory(Story<P> story) {
			try {
				idColumn.RemoveStory(story);
				foreach(var kvp in columns) kvp.Value.RemoveStory(story);
			} catch (System.Exception e) { Dj.Errorf("RemoveStory failed\n{0}",e); return; }
			story.storyteller = null;
			stories.Remove(story);
		}
		public void Clear() {
			idColumn.Clear();
			foreach(var kvp in columns) kvp.Value.Clear();
			stories.Clear();
		}
		public Story<P> GetStory<K>(byte columnId, K key) {
			if (!columns.ContainsKey(columnId)) throw Dj.Crashf("GetStory exception: no column id {0} has been defined",columnId);
			if (!(columns[columnId] is Column<K,P>)) throw Dj.Crashf("GetStory exception: column id {0} does not have key type {1}",columnId,typeof(K));
			return ((Column<K,P>)columns[columnId]).GetStory(key);
		}
		public Story<P> GetStory(int id) {
			return idColumn.GetStory(id);
		}
		public void DoEach(System.Action<Story<P>> action) {
			foreach(var story in stories) action(story);
		}
		HashSet<Story<P>> stories;
		Dictionary<byte,Column<P>> columns;
		Column<int, P> idColumn;
	}



	abstract class Column<P> where P : IPage {
		abstract internal void AddStory(Story<P> story);
		abstract internal void RemoveStory(Story<P> story);
		abstract internal void Clear();
	}
	class Column<K,P> : Column<P> where P : IPage {

		override internal void AddStory(Story<P> story) {
			this.stories.Add(GetColValue(story.GetPage()), story);
		}
		override internal void RemoveStory(Story<P> story) {
			this.stories.Remove(GetColValue(story.GetPage()));
		}
		override internal void Clear() {
			this.stories.Clear();
		}
		internal Story<P> GetStory(K key) {
			if (this.stories.ContainsKey(key)) return this.stories[key];
			else return null;
		}

		internal Column (StorytellerUtils.ColumnValueGetter<K,P> GetColValue) {
			this.GetColValue = GetColValue;
			this.stories = new Dictionary<K, Story<P>>();
		}
		StorytellerUtils.ColumnValueGetter<K,P> GetColValue;
		Dictionary<K,Story<P>> stories;
	}
}