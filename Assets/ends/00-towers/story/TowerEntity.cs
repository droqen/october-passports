using UnityEngine;
using System.Collections;

namespace ends.tower.story
{

    using passport.story3;
    using navdi3;

    public class TowerEntity : Story
    {
        public const short OPCODE = 735;
        override public short op { get { return OPCODE; } }

        public twin? LastTrackedWorldPos;

        public int EntityId
        {
            get { return Get<int>("eid"); }
            set { Set("eid", value); }
        }

        public twin WorldPos
        {
            get { return Get<twin>("wps"); }
            set { Set("wps", value); }
        }

        public twin Position
        {
            get { return Get<twin>("pos"); }
            set { Set("pos", value); }
        }

        public TowerEntity(byte[] bytes) : base(bytes) { }
        public TowerEntity(string address) : base(address) { }
    }

}