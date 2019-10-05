using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.story3.post
{

    using passport.crunch;

    [System.Serializable]
    public struct ServeStory {
        public const short op = 5401;
        public short storyOPCODE;
        public byte[] storyBytes;
        public ServeStory(Story story) { this.storyOPCODE = story.OPCODE; this.storyBytes = story.ToBytes(); }
    }
    [System.Serializable]
    public struct ServeStoryDelta
    {
        public const short op = 5404;
        public Pages delta;
    }

}