using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using passport.link;

namespace ends.tower
{
    using mess;

    public class TowerQServer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var link = GetComponent<ServersideLink>();
            link.SetPostHandler<Test, Test_Reply>(Test.OPCODE, (post) =>
            {
                Debug.Log("Server got message " + post.action.message);
                post.reply = new Test_Reply { response = "Yep, it's the server. Got it.", };
                post.Reply();
            });
        }
    }

}
