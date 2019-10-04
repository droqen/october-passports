namespace passport.story {
public struct StoryOptions {
	public int maxFutureDeltas;
	public bool storePastDeltas;
	public bool deltaListener;
	public bool deltaSpawner;
	public static StoryOptions Spawner {get{return new StoryOptions{deltaSpawner=true,};}}
	public static StoryOptions Listener {get{return new StoryOptions{deltaListener=true,};}}
}
}