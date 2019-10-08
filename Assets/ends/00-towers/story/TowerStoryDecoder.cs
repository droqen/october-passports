using UnityEngine;
using System.Collections;

namespace ends.tower
{

    using passport.story3;
    using story;

    public class TowerStoryDecoder : IStorydecoder
    {
        public TowerStoryDecoder()
        {
            // ok?
        }

        public Story Decode(Pages pages)
        {
            switch(pages.op)
            {
                case TowerZone.OPCODE: return new TowerZone(pages);
                case TowerEntity.OPCODE: return new TowerEntity(pages);
                default: return null;
            }
        }
    }

}