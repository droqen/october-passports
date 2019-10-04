using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.story3
{

    using crunch;

    public class Story
    {
        Dictionary<string, byte[]> lastWrittenData;
        Pages pages;

        public T Get<T>(string key) { return Capn.Decrunchatize<T>(this.pages.data[key]); }
        public void Set<T>(string key, T value) { this.pages.data[key]=Capn.Crunchatize(value); }

        public Storyteller storyteller { get; private set; }
        public string address { get; private set; }

        public byte[] ToBytes()
        {
            return Capn.Crunchatize(this.pages);
        }
        public Story(byte[] bytes)
        {
            Pages loadedPages = Capn.Decrunchatize<Pages>(bytes);
            this.address = loadedPages.address;
            //this.unwrittenData = new Dictionary<string, byte[]>();
            this.pages = loadedPages;
        }
        public Story(string address)
        {
            // every story needs a 100% unique *address*.
            this.address = address;
            //this.unwrittenData = new Dictionary<string, byte[]>();
            this.pages = new Pages { address = address, data = new Dictionary<string, byte[]>(), };
        }

        public void SetStoryteller(Storyteller storyteller)
        {
            this.storyteller = storyteller;
            if (storyteller.isAuthor) this.lastWrittenData = this.pages.data; // initialize lastWrittenData
        }

        public Pages WriteChanges()
        {
            Pages delta = new Pages();

            if (storyteller == null) Dj.Crashf("Story '{0}' has no storyteller.", this.address);
            if (storyteller.isAuthor)
            {
                storyteller.Write(this);

                // TODO: make this more efficient: only write the changes (hence the name 'delta')
                delta.address = this.address;
                delta.data = this.pages.data;

                lastWrittenData.Clear();
                foreach (var kvp in this.pages.data) lastWrittenData.Add(kvp.Key, kvp.Value);
            } else
            {
                Dj.Warnf("Story '{0}' can't WriteChanges, it's attached a non-author storyteller.", this.address);
            }

            return delta;
        }
        public void ReadChanges(Pages delta)
        {
            if (delta.address != this.address) Dj.Crashf("Story '{0}' can't read from mismatched delta '{1}'.", this.address, delta.address);
            if (storyteller == null) Dj.Crashf("Story '{0}' has no storyteller.", this.address);
            if (storyteller.isAuthor) Dj.Warnf("Story '{0}' can't ReadChanges, it's an author.", this.address);
            else
                foreach (var kvp in delta.data)
                    this.pages.data[kvp.Key] = kvp.Value;
        }
    }

    public struct Pages
    {
        public string address;
        public Dictionary<string, byte[]> data;
    }

}