using UnityEngine;
using System.Collections;

namespace passport.sessions.post
{

    [System.Serializable]
    public struct Login
    {
        public const short op = 5501;
        public string username;
    }
    [System.Serializable]
    public struct Login_Reply
    {
        public bool okay;
    }

}