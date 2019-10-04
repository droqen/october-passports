using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.story3.post
{

    [System.Serializable]
    public struct ServeStory {
        public const short op = 5401;
        public byte[] storyBytes;
        public Story story { get { return new Story(storyBytes); } }
        public ServeStory(Story story) { this.storyBytes = story.ToBytes(); }
    }
    [System.Serializable]
    public struct ServeStoryDelta
    {
        public const short op = 5404;
        public Pages delta;
    }

}