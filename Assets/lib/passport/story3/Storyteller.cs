using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.story3 {

    public class Storyteller
    {
        public bool isAuthor { get; private set; }
        Dictionary<string, Story> stories;
        public Dictionary<short, List<Storyfan>> storyfans;
        public Storyteller(bool isAuthor)
        {
            this.stories = new Dictionary<string, Story>();
            this.storyfans = new Dictionary<short, List<Storyfan>>();
            this.isAuthor = isAuthor;
        }
        public void Write(Story story)
        {
            if (!isAuthor) Dj.Crashf("Non-Authors can't Write. Failed to Write Story '{0}'.", story.address);
            if (story.SetStoryteller(this)) {
                this.stories.Add(story.address, story);
                StoryChanged(story);
            } else if (this.stories[story.address] == story) {
                StoryChanged(story);
            }
            else
            {
                Dj.Warnf("WARNING: Write story@{0} failed, please implement", story.address);
            }
        }
        public Story Read(Story story)
        {
            if (isAuthor) Dj.Crashf("Authors can't Read. Failed to Read Story '{0}'.", story.address);
            story.SetStoryteller(this);
            this.stories[story.address] = story;
            StoryChanged(story);
            return story;
        }
        public Story Read(Pages delta)
        {
            if (isAuthor) Dj.Crashf("Authors can't Read. Failed to Read delta Pages '{0}'.", delta.address);
            if (this.stories.TryGetValue(delta.address, out var story))
            {
                story.SetStoryteller(this);
                story.ReadChanges(delta);
                StoryChanged(story);
                return story;
            }
            else
            {
                Dj.Warnf("Couldn't find matching story for delta Pages '{0}'.", delta.address);
                return null;
            }
        }
        public void Erase(string address)
        {
            this.stories.Remove(address);
        }

        public MyStory Get<MyStory>(string address) where MyStory : Story
        {
            return (MyStory)stories[address];
        }

        public void AddStoryfan(short op, Storyfan storyfan)
        {
            if (!storyfans.ContainsKey(op)) storyfans.Add(op, new List<Storyfan>());
            storyfans[op].Add(storyfan);
        }

        void StoryChanged(Story story)
        {
            if (storyfans.TryGetValue(story.op, out var justMyFans))
            {
                foreach (var fan in justMyFans) fan.StoryChanged(story);
            }
        }

    }

}