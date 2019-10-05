using UnityEngine;
using System.Collections;

namespace passport.sessions
{

    using passport.story3;

    public class Session : Story
    {
        public const short OPCODE = 1;
        override public short op { get { return OPCODE; } }

        public int SessionId
        {
            get { return Get<int>("sid"); }
            set { Set("sid", value); }
        }
        public string Username
        {
            get { return Get<string>("unm"); }
            set { Set("unm", value); }
        }
        public int EntityId
        {
            get { return Get<int>("ent"); }
            set { Set("ent", value); }
        }

        public int PeerId;

        public Session(byte[] bytes) : base(bytes) { }
        public Session(string address) : base(address) { }
    }

}