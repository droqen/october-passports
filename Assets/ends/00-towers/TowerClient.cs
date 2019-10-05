using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ends.tower
{

    using passport;
    using passport.link;
    using passport.sessions;
    using passport.story3;

    using post;
    using story;
    using navdi3;

    public class TowerClient : MonoBehaviour
    {
        public TMP_Text tmp_display;

        ClientsideLink link;
        ClientsideSessions sessions;
        Session currentSession;
        TowerZone currentZone;
        TowerEntity myEntity;
        HashSet<TowerEntity> visibleEntities = new HashSet<TowerEntity>();

        private void Start()
        {
            link = GetComponent<ClientsideLink>();
            sessions = new ClientsideSessions(link);
            link.AttemptConnection(success =>
            {
                if (success)
                {
                    sessions.DoLogin("droqen");
                }
                else
                {
                    Dj.Warnf("Login failed. TODO: Implement retry");
                }
            });
            sessions.AddStoryfan(Session.OPCODE, new LambdaStoryfan<Session>(session =>
            {
                if (currentSession == null || currentSession.Username != session.Username)
                {
                    currentSession = session;
                    link.Post<OK>(RequestStories.OPCODE, new RequestStories { message = "Hello from " + currentSession.Username, },
                        reply =>
                        {

                        },
                        failStatus =>
                        {
                            Dj.Tempf("RequestStories rejected with status {0}. Recommended to either request again, or disconnect.", failStatus);
                        });
                }

                Dj.Tempf("My session says my name is '{0}'", currentSession.Username);
            }));

            sessions.AddStorydecoder(TowerZone.OPCODE, b => { return new TowerZone(b); });
            sessions.AddStoryfan(TowerZone.OPCODE, new LambdaStoryfan<TowerZone>(zone =>
            {
                currentZone = zone;

                visibleEntities.Clear();
            }));

            sessions.AddStorydecoder(TowerEntity.OPCODE, b => { return new TowerEntity(b); });
            sessions.AddStoryfan(TowerEntity.OPCODE, new LambdaStoryfan<TowerEntity>(ent =>
            {
                bool its_me = false;
                try
                {
                    if (currentSession.EntityId == ent.EntityId)
                    {
                        myEntity = ent;
                        its_me = true;
                    }
                }
                catch { }

                if (!its_me)
                {
                    if (ent.WorldPos == currentZone.WorldPos) visibleEntities.Add(ent);
                    else visibleEntities.Remove(ent);
                }
            }));

        }

        float moveInputPending = 0.0f;

        private void Update()
        {
            if (moveInputPending > 0.0f) moveInputPending -= Time.deltaTime;
            else
            {
                twin move = Pin.GetTwinDown();
                if (move.taxicabLength == 1)
                {
                    moveInputPending = 1.0f;
                    link.Post<OK>(WorldMove.OPCODE, new WorldMove { dir = move },
                        ok =>
                        {
                            //moveInputPending = 0.0f;
                        },
                        failReason =>
                        {
                            moveInputPending = 0.0f;
                        });
                }
            }

            if (currentSession == null || currentZone == null || myEntity == null)
            {
                //Dj.Tempf("{0}--{1}--{2}", currentSession, currentZone, myEntity);
                tmp_display.text = "connecting...";
            }
            else
            {
                tmp_display.text = string.Format("Your session:@{0}\nIn zone '{1}'@{2}\nYour position within the world: {3}\n# of other ents here: {4}",
                    currentSession.address, currentZone.ZoneName, currentZone.WorldPos, myEntity.Position, visibleEntities.Count);
            }

        }
    }

}