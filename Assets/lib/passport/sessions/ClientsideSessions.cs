using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.sessions
{

    using link;
    using sessions.post;
    using story3;
    using story3.post;

    public class ClientsideSessions
    {
        public Session session;

        ClientsideLink link;
        Storyteller storyteller;
        HashSet<IStoryfan> fans;
        Dictionary<short, System.Func<byte[], Story>> storyDecodeFunctions;

        public ClientsideSessions(ClientsideLink link)
        {
            this.link = link;
            this.storyteller = new Storyteller(isAuthor: false);
            this.fans = new HashSet<IStoryfan>();
            this.storyDecodeFunctions = new Dictionary<short, System.Func<byte[], Story>>();
            AddStorydecoder(1, b => { return new Session(b); });
            this.link.SetPostHandler<ServeStory>(ServeStory.op, (post) =>
                {
                    var story = storyteller.Read(Decode(post));
                    if (story!=null) PingStoryfans(story);
                }
            );
            this.link.SetPostHandler<ServeStoryDelta>(ServeStoryDelta.op, (post) =>
                {
                    var story = storyteller.Read(post.delta);
                    if (story!=null) PingStoryfans(story);
                }
            );
        }

        public void DoLogin(string username)
        {
            this.link.Post<Login_Reply>(Login.op, new Login { username = username },
                reply => { }, rejectionStatus => { });
        }

        public void AddStoryfan(IStoryfan fan)
        {
            fans.Add(fan);
        }
        public void PingStoryfans(Story story)
        {
            foreach(var fan in fans) fan.StoryChanged(story);
        }

        public Story Decode(ServeStory serveAction)
        {
            if (storyDecodeFunctions.TryGetValue(serveAction.storyOPCODE, out var fn))
            {
                return fn(serveAction.storyBytes);
            } else
            {
                throw Dj.Crashf("Decode failed on Story with unknown OPCODE '{0}' at address '{1}'.", serveAction.storyOPCODE, new Session(serveAction.storyBytes).address);
            }
        }
        public void AddStorydecoder(short op, System.Func<byte[], Story> fn)
        {
            this.storyDecodeFunctions.Add(op, fn);
        }
    }

}