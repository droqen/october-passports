using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ends.tower
{

    using passport;
    using passport.link;
    using passport.sessions;
    using passport.story3;

    using mess;

    public class TowerClient : MonoBehaviour
    {
        ClientsideLink link;
        ClientsideSessions sessions;
        Session currentSession;
        private void Start()
        {
            link = GetComponent<ClientsideLink>();
            sessions = new ClientsideSessions(link);
            link.AttemptConnection(success =>
            {
                if (success)
                {
                    sessions.DoLogin("droqen");
                } else
                {
                    Dj.Warnf("Login failed. TODO: Implement retry");
                }
            });
            sessions.AddStoryfan(new LambdaStoryfan(story=>
            {

                if (story is Session)
                {
                    var session = (Session)story;
                    if (currentSession == null || currentSession.Username != session.Username)
                    {
                        currentSession = session;
                        link.Post<Test_Reply>(Test.OPCODE, new Test { message = "Hello from " + currentSession.Username, },
                            reply =>
                            {
                                Dj.Tempf("Client received Test_Reply '{0}'!", reply.response);
                            },
                            failStatus =>
                            {
                                Dj.Tempf("Test post rejected with status {0}.", failStatus);
                            });
                    }
                }

            }));
        }
    }

}