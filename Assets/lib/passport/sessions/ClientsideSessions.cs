using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace passport.sessions
{

    using crunch;
    using link;
    using sessions.post;
    using story3;
    using story3.post;

    public class ClientsideSessions
    {
        public Session session;

        ClientsideLink link;
        public Storyteller storyteller { get; private set; }
        List<IStorydecoder> decoders;

        public ClientsideSessions(ClientsideLink link)
        {
            this.link = link;
            this.storyteller = new Storyteller(isAuthor: false);
            this.decoders = new List<IStorydecoder>();
            PushStorydecoder(new SessionDecoder());
            this.link.SetPostHandler<ServeStory>(ServeStory.op, (post) =>
                {
                    var story = storyteller.Read(Decode(post));
                }
            );
            this.link.SetPostHandler<ServeStoryDelta>(ServeStoryDelta.op, (post) =>
                {
                    var story = storyteller.Read(post.delta);
                }
            );
        }

        public void DoLogin(string username)
        {
            this.link.Post<Login_Reply>(Login.op, new Login { username = username },
                reply => { }, rejectionStatus => { });
        }

        public void AddStoryfan(short op, Storyfan storyfan)
        {
            storyteller.AddStoryfan(op, storyfan);
        }

        public Story Decode(ServeStory serveAction)
        {
            Pages pages = Capn.Decrunchatize<Pages>(serveAction.storyBytes);
            foreach (var decoder in decoders)
            {
                var story = decoder.Decode(pages);
                if (story != null) return story;
            }

            throw Dj.Crashf("Decode failed on Story with unknown OPCODE '{0}' at address '{1}'. (OPCODE was not caught by any pushed IStorydecoder)", pages.op, pages.address);
        }
        public void PushStorydecoder(IStorydecoder decoder)
        {
            this.decoders.Insert(0, decoder);
        }
    }

}