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

        public ClientsideSessions(ClientsideLink link)
        {
            this.link = link;
            this.storyteller = new Storyteller(isAuthor: false);
            this.fans = new HashSet<IStoryfan>();

            this.link.SetPostHandler<ServeStory>(ServeStory.op, (post) =>
                {
                    var story = storyteller.Read(post.story);
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
    }

}