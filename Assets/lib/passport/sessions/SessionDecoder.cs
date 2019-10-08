using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.sessions
{

    using link;
    using sessions.post;
    using story3;
    using story3.post;

    public class SessionDecoder : IStorydecoder
    {
        public Story Decode(Pages pages)
        {
            switch(pages.op)
            {
                case 1: return new Session(pages);
                default: return null;
            }
        }
    }

}