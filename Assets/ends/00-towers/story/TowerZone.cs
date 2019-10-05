using UnityEngine;
using System.Collections;

namespace ends.tower.story
{

    using passport.story3;
    using navdi3;

    public class TowerZone : Story
    {
        public const short OPCODE = 133;
        override public short op { get { return OPCODE; } }

        public string ZoneName
        {
            get { return Get<string>("znm"); }
            set { Set("znm", value); }
        }

        public twin WorldPos
        {
            get { return Get<twin>("pos"); }
            set { Set("pos", value); }
        }

        public TowerZone(byte[] bytes) : base(bytes) { }
        public TowerZone(string address) : base(address) { }
    }

}