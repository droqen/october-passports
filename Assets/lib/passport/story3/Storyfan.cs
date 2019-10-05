using UnityEngine;
using System.Collections;

namespace passport.story3
{

    abstract public class Storyfan
    {
        abstract public void StoryChanged(Story story);
    }
    abstract public class Storyfan<MyStory> : Storyfan where MyStory : Story
    {
        override public void StoryChanged(Story story) { StoryChanged((MyStory)story); }
        abstract public void StoryChanged(MyStory story);
    }

}