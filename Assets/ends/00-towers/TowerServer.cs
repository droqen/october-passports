using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ends.tower
{

    using passport;
    using passport.link;
    using passport.sessions;

    using mess;

    public class TowerServer : MonoBehaviour
    {
        ServersideLink link;
        ServersideSessions sessions;
        private void Start()
        {
            link = GetComponent<ServersideLink>();
            sessions = new ServersideSessions(link);
            link.SetPostHandler<Test, Test_Reply>(Test.OPCODE, poster =>
            {
                poster.reply.response = string.Format(
                    "Server heard '{0}' from client", 
                    poster.action.message);
                poster.Reply();
            });
        }
    }

}