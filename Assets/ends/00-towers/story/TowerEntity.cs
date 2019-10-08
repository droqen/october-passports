using UnityEngine;
using System.Collections;

namespace ends.tower.story
{

    using passport.story3;
    using navdi3;

    [System.Serializable]
    public class TowerEntity : Story
    {
        public const short OPCODE = 735;
        override public short op { get { return OPCODE; } }

        [System.NonSerialized] public twin? LastTrackedWorldPos;

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

        public TowerEntity(Pages pages): base(pages) { }
        public TowerEntity(string address) : base(OPCODE, address) { }
    }

}