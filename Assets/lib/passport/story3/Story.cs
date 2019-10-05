using System.Collections.Generic;

namespace passport.story3
{

    using crunch;

    abstract public class Story
    {
        abstract public short OPCODE { get; }

        Dictionary<string, byte[]> lastWrittenData;
        Pages pages;

        public T Get<T>(string key) { return Capn.Decrunchatize<T>(this.pages.data[key]); }
        public void Set<T>(string key, T value) { this.pages.data[key] = Capn.Crunchatize(value); }

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

        public bool SetStoryteller(Storyteller storyteller)
        {
            if (this.storyteller == null)
            {
                this.storyteller = storyteller;
                if (storyteller.isAuthor) MemorizeLastWrittenPagesData();
                return true;
            } else
            {
                return false;
            }
        }

        public void MemorizeLastWrittenPagesData()
        {
            if (lastWrittenData != null) lastWrittenData.Clear();
            else lastWrittenData = new Dictionary<string, byte[]>();
            foreach (var kvp in this.pages.data) lastWrittenData.Add(kvp.Key, kvp.Value);
        }

        public Pages WriteChanges()
        {
            Dj.Tempf("Writing changes");

            Pages delta = new Pages();

            if (storyteller == null) throw Dj.Crashf("Story '{0}' has no storyteller.", this.address);
            if (storyteller.isAuthor)
            {
                storyteller.Write(this);

                delta.address = this.address;
                delta.data = new Dictionary<string, byte[]>();
                foreach (var kvp in this.pages.data)
                {
                    if (lastWrittenData.TryGetValue(kvp.Key, out var lastWrittenBytes))
                    {
                        if (lastWrittenBytes.Equals(kvp.Value))
                        {
                            // skip
                        } else
                        {
                            delta.data.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
                MemorizeLastWrittenPagesData();
            } else
            {
                Dj.Warnf("Story '{0}' can't WriteChanges, it's attached a non-author storyteller.", this.address);
            }

            return delta;
        }
        public void ReadChanges(Pages delta)
        {
            if (delta.address != this.address) throw Dj.Crashf("Story '{0}' can't read from mismatched delta '{1}'.", this.address, delta.address);
            if (storyteller == null) throw Dj.Crashf("Story '{0}' has no storyteller.", this.address);
            if (storyteller.isAuthor) Dj.Warnf("Story '{0}' can't ReadChanges, it's an author.", this.address);
            else
                foreach (var kvp in delta.data)
                    this.pages.data[kvp.Key] = kvp.Value;
        }

        public void DebugDump()
        {
            string keyList = "";

            foreach (var kvp in this.pages.data)
                keyList += kvp.Key + ", ";

            Dj.Tempf("{1} ... debug dumping story@{0}",
                this.address,
                keyList
                );
        }
    }

    [System.Serializable]
    public struct Pages
    {
        public string address;
        public Dictionary<string, byte[]> data;
    }

}