using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.story3 {

    public class Storyteller
    {
        public bool isAuthor { get; private set; }
        Dictionary<string, Story> stories;
        public Storyteller(bool isAuthor)
        {
            this.stories = new Dictionary<string, Story>();
            this.isAuthor = isAuthor;
        }
        public void Write(Story story)
        {
            Dj.Tempf("Write story {0}", story.address);
            if (!isAuthor) Dj.Crashf("Non-Authors can't Write. Failed to Write Story '{0}'.", story.address);
            if (story.SetStoryteller(this))
            {
                this.stories.Add(story.address, story);
            }
        }
        public Story Read(Story story)
        {
            if (isAuthor) Dj.Crashf("Authors can't Read. Failed to Read Story '{0}'.", story.address);
            story.SetStoryteller(this);
            this.stories[story.address] = story;
            return story;
        }
        public Story Read(Pages delta)
        {
            if (isAuthor) Dj.Crashf("Authors can't Read. Failed to Read delta Pages '{0}'.", delta.address);
            if (this.stories.TryGetValue(delta.address, out var story))
            {
                story.SetStoryteller(this);
                story.ReadChanges(delta);
                return story;
            }
            else
            {
                Dj.Warnf("Couldn't find matching story for delta Pages '{0}'.", delta.address);
                return null;
            }
        }
    }

}