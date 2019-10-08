using UnityEngine;
using System.Collections;

namespace ends.tower.story
{

    using passport.story3;
    using navdi3;

    [System.Serializable]
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

        public byte[] TileGrid
        {
            get { return Get<byte[]>("map"); }
            set { Set("map", value); }
        }

        public byte GetTile(byte X, byte Y)
        {
            return TileGrid[X+Y*9];
        }

        public void SetTile(byte X, byte Y, byte B)
        {
            TileGrid[X+Y*9] = B;
        }

        public TowerZone(Pages pages) : base(pages) { }
        public TowerZone(string address) : base(OPCODE, address) { TileGrid = new byte[9*9]; }
    }

}