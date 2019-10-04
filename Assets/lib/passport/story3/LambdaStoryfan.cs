using UnityEngine;
using System.Collections;

namespace passport.story3 {

    public class LambdaStoryfan : IStoryfan
    {
        System.Action<Story> onStoryChanged;
        public void StoryChanged(Story story) {
            this.onStoryChanged(story);
        }
        public LambdaStoryfan(System.Action<Story> onStoryChanged) {
            this.onStoryChanged = onStoryChanged;
        }
    }

}