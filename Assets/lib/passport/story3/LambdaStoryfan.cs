using UnityEngine;
using System.Collections;

namespace passport.story3 {

    public class LambdaStoryfan<MyStory> : Storyfan<MyStory> where MyStory : Story
    {
        System.Action<MyStory> onStoryChanged;
        override public void StoryChanged(MyStory story) {
            this.onStoryChanged(story);
        }
        public LambdaStoryfan(System.Action<MyStory> onStoryChanged) {
            this.onStoryChanged = onStoryChanged;
        }
    }

}