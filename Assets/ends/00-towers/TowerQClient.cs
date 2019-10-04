using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using passport.link;
using passport.sessions;
using passport.sessions.post;
using passport.story3;

namespace ends.tower
{
    using mess;

    public class TowerQClient : MonoBehaviour
    {

        /// <summary>
        /// View
        /// </summary>

        public TMP_Text tmpTypedInput;
        public TMP_Text tmpPrintedOutput;
        private string typedInput = "";

        void ConsClear()
        {
            tmpPrintedOutput.text = "";
        }
        void ConsPrint(string message, params object[] printf_args)
        {
            if (printf_args != null && printf_args.Length > 0)
            {
                tmpPrintedOutput.text += "\n" + string.Format(message, printf_args);
            }
            else
            {
                tmpPrintedOutput.text += "\n" + message;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            ConsClear();
            ConsPrint("Connecting...");
            Setup();
            tmpTypedInput.text = string.Format(">{0}_", typedInput);
        }

        // Update is called once per frame
        void PostTestToServer()
        {
            var link = GetComponent<ClientsideLink>();
            link.Post<Test_Reply>(
                opCode: Test.OPCODE,
                serializableObject: new Test { message = "Hello from client", },
                successCallback: (reply) =>
                {
                    ConsPrint("Client got response: {0}", reply.response);
                },
                nonSuccessCallback: (status) =>
                {
                    ConsPrint("Test failed, status: {0}", status);
                }
            );
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (typedInput.Length > 0)
                {
                    if (typedInput == "login")
                    {
                        //session.POST_ConnectAndLogin("droqen", ()=> { ConsPrint("login failed"); });
                    }

                    ConsPrint(">{0}", typedInput);
                    typedInput = "";
                    tmpTypedInput.text = string.Format(">{0}_", typedInput);
                }
            }
            else foreach(var c in Input.inputString)
            {
                if (char.IsControl(c) || Input.GetKey(KeyCode.LeftWindows) || Input.GetKey(KeyCode.RightWindows) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    switch((KeyCode)c)
                    {
                        case KeyCode.Backspace:
                            typedInput = typedInput.Substring(0, typedInput.Length - 1);
                            break;
                    }
                }
                else
                {
                    typedInput += c;
                }
                tmpTypedInput.text = string.Format(">{0}_", typedInput);
            }
        }


        /// <summary>
        /// Model
        /// </summary>

        ClientsideLink link;
        ClientsideSessions sessions;

        public void Setup()
        {
            link = GetComponent<ClientsideLink>();
            link.AttemptConnection(
                (success) =>
                {
                    if (success)
                    {
                        this.PostTestToServer();
                        ConsPrint("Connected to Server.");
                    }
                    else
                    {
                        ConsPrint("Test failed, couldn't even connect to Server.");
                    }
                }
            );

            sessions = new ClientsideSessions(link);
            sessions.AddStoryfan(new LambdaStoryfan(story =>
                {

                    if (story is Session)
                    {
                        var session = (Session)story;

                        ConsPrint("You are session #{0}", session.SessionId);
                    }

                }
            ));
        }
        public void TestLogin()
        {
            link.Post(Login.op, new Login { username = "droqen", }, null);
        }
    }

}
