using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.sessions
{

    using link;
    using sessions.post;
    using story3;
    using story3.post;

    public class ServersideSessions
    {

        virtual public int GenerateUniqueSessionId()
        {
            return Random.Range(int.MinValue, int.MaxValue);
        }

        ServersideLink link;
        public Storyteller storyteller { get; private set; }
        public Dictionary<int, Session> peerSessions { get; private set; }
        Dictionary<short, StoryfanListenerAdder> storyListenerFunctions;

        public bool UsingPeer(int peerid, out Session session)
        {
            if (peerSessions.TryGetValue(peerid, out session))
            {
                return true;
            }
            else
            {
                Dj.Errorf("Peer {0} could not be used.", peerid);
                return false;
            }
        }

        public ServersideSessions(ServersideLink link)
        {
            this.link = link;
            this.storyteller = new Storyteller(isAuthor: true);
            this.peerSessions = new Dictionary<int, Session>();
            this.storyListenerFunctions = new Dictionary<short, StoryfanListenerAdder>();

            this.SetFunctionToAddStoryListeners<Session>(Session.OPCODE, (session, listeners) =>
            {
                // Sessions listen to themselves
                listeners.Add(session);
            });

            link.OnPeerConnect = (peer) =>
            {
                // this session is a ghost.
                var sessionId = GenerateUniqueSessionId();
                var session = new Session("s/" + sessionId);
                session.EntityId = 0;
                session.SessionId = sessionId;
                session.Username = "";
                session.PeerId = peer.Id;
                this.storyteller.Write(session);

                this.peerSessions.Add(session.PeerId, session);
            };
            link.OnPeerDisconnect = (peer) =>
            {
                // wipe my session
                if (UsingPeer(peer.Id, out var session))
                {
                    // disconnect session
                    peerSessions.Remove(peer.Id);
                }
            };
            link.SetPostHandler<Login, Login_Reply>(Login.op, (posted) =>
                {
                    if (UsingPeer(posted.Peer.Id, out var session))
                    {
                        if (posted.action.username != "" && session.Username == "")
                        {
                            session.Username = posted.action.username;
                            posted.reply.okay = true;
                            posted.Reply();
                            session.WriteChanges(); // send to anyone who should be able to see this session story (should only be the peer)
                        }
                        else
                        {
                            posted.reply.okay = false;
                            posted.Reply();
                        }
                    }
                    else
                    {
                        posted.Reject(Barebones.Networking.ResponseStatus.NotConnected);
                    }
                }
            );
            // do nothing?
        }

        public void SetFunctionToAddStoryListeners<MyStory>(short op, System.Action<MyStory, HashSet<Session>> fn)
            where MyStory : Story
        {
            if (storyListenerFunctions.TryGetValue(op, out var adderfan))
            {
                ((StoryfanListenerAdder<MyStory>)adderfan).addListenersFunction = fn;
            } else
            {
                var fan = new StoryfanListenerAdder<MyStory>
                {
                    link = link,
                    addListenersFunction = fn,
                };
                storyListenerFunctions.Add(op, fan);
                storyteller.AddStoryfan(op, fan);
            }
        }

        abstract class StoryfanListenerAdder : Storyfan
        {
            
        }
        class StoryfanListenerAdder<MyStory> : StoryfanListenerAdder where MyStory: Story
        {
            public ServersideLink link;
            public System.Action<MyStory, HashSet<Session>> addListenersFunction;
            public override void StoryChanged(Story story)
            {
                HashSet<Session> listeners = new HashSet<Session>();
                addListenersFunction((MyStory)story, listeners);
                foreach (var session in listeners)
                {
                    Dj.Tempf("Server posting story@{0} to peer#{1}", story.address, session.PeerId);
                    link.Post(session.PeerId, ServeStory.op, new ServeStory(story));
                }
            }
        }

    }

}
