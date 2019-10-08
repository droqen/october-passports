using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ends.tower
{

    using passport;
    using passport.link;
    using passport.crunch;
    using passport.sessions;

    using post;
    using story;
    using passport.story3;
    using passport.story3.post;
    using navdi3;

    public class TowerServer : MonoBehaviour
    {
        public float backupFreqInSeconds = 100f;
        public float backupTimer;

        WorldData worldData;
        ServersideLink link;
        ServersideSessions sessions;
        Dictionary<string, TowerEntity> sessionToTowerEntities;
        EntGenerator entGenerator;
        TowerStoryDecoder decoder;
        private void Start()
        {

            string s = "hello";
            Dj.Temp("Begin size test");
            byte[] b = Capn.Crunchatize(s);
            for (var i = 0; i < 5; i++)
            {
                Dj.Tempf("Iter #{0}, size {1}", i, b.Length);
                b = Capn.Crunchatize(b);
            }

            backupTimer = backupFreqInSeconds;

            worldData = new WorldData(true);
            link = GetComponent<ServersideLink>();
            sessions = new ServersideSessions(link);
            sessionToTowerEntities = new Dictionary<string, TowerEntity>();
            entGenerator = new EntGenerator();
            decoder = new TowerStoryDecoder();

            sessions.SetFunctionToAddStoryListeners<TowerEntity>(TowerEntity.OPCODE, this.GetWhoCanSeeMe_UpdateTracking);
            sessions.SetFunctionToAddStoryListeners<TowerZone>(TowerZone.OPCODE, this.GetWhosListeningHere);

            sessions.storyteller.AddStoryfan(Session.OPCODE, new LambdaStoryfan<Session>(session =>
            {
                if (sessionToTowerEntities.ContainsKey(session.address))
                {
                    if (session.PeerId < 0)
                    {
                        // you're disconnected but your body is still in the world.
                        // bye now!

                    }
                }
                else
                {
                    if (session.PeerId >= 0)
                    {
                        // you're connected but don't have a body. let's remedy that!
                        // int EntityId = (nextTowerEntityId++);

                        var playerTowerEntity = entGenerator.NewPlayerEntity();
                        //playerTowerEntity.EntityId = EntityId;
                        //playerTowerEntity.WorldPos = twin.zero;
                        //playerTowerEntity.Position = twin.zero;

                        session.EntityId = playerTowerEntity.EntityId;

                        sessionToTowerEntities[session.address] = playerTowerEntity;
                        session.WriteChanges();
                        sessions.storyteller.Write(playerTowerEntity); // suddenly, you're embodied
                    }
                }
            }
            ));

            link.SetPostHandler<RequestStories, OK>(RequestStories.OPCODE, poster =>
            {
                if (sessions.UsingPeer(poster.Peer.Id, out var session))
                {
                    Dj.Tempf("Server heard request from client");
                    poster.Reply();
                    PushStoriesToSession(session);
                }
            });

            link.SetPostHandler<WorldMove, OK>(WorldMove.OPCODE, poster =>
            {
                try
                {
                    sessions.UsingPeer(poster.Peer.Id, out var session);
                    var ent = sessionToTowerEntities[session.address];
                    ent.WorldPos += poster.action.dir;
                    ent.WriteChanges();

                    poster.Reply();

                    PushEntitiesToSession(session);
                } catch {
                    poster.Reject(Barebones.Networking.ResponseStatus.Failed);
                }
            });

            SetupStartingZones();
        }

        private void Update()
        {
            backupTimer -= Time.deltaTime;
            if (backupTimer <= 0)
            {
                backupTimer = backupFreqInSeconds;
                CreateWorldBackup();
            }
        }

        public void GetWhoCanSeeMe_UpdateTracking(TowerEntity ent, HashSet<Session> listeners)
        {
            var currentZone = this.worldData.towerZones[ent.WorldPos];
            GetWhosListeningHere(currentZone, listeners);

            if (!ent.LastTrackedWorldPos.HasValue || ent.LastTrackedWorldPos.Value != ent.WorldPos)
            {
                Dj.Tempf("Changed position");
                if (ent.LastTrackedWorldPos.HasValue)
                {
                    var previousZone = this.worldData.towerZones[ent.LastTrackedWorldPos.Value];
                    GetWhosListeningHere(previousZone, listeners);
                }
                foreach (var session in listeners) if (session.EntityId == ent.EntityId) link.Post(session.PeerId,
                    ServeStory.op, new ServeStory(currentZone));

                ent.LastTrackedWorldPos = ent.WorldPos; // now, update LastTrackedWorldPos.

                this.worldData.entitiesById[ent.EntityId] = ent;
            }
        }
        public void GetWhosListeningHere(TowerZone zone, HashSet<Session> listeners)
        {
            foreach (var kvp in sessionToTowerEntities) try
                {
                    if (kvp.Value.WorldPos == zone.WorldPos) listeners.Add(sessions.storyteller.Get<Session>(kvp.Key));
                }
                catch { }
        }

        void SetupStartingZones()
        {
            for(int X = -5; X <= 5; X++) for(int Y = -5; Y <= 5; Y++)
                {
                    var zone = new TowerZone("t/" + X + "," + Y);
                    zone.WorldPos = new twin(X, Y);
                    zone.ZoneName = "nowhere";
                    this.worldData.towerZones[zone.WorldPos] = zone;
                    sessions.storyteller.Write(zone);

                    for (int i = Random.Range(0,3); i<4; i++)
                    {
                        var ent = entGenerator.NewRockEntity();
                        ent.WorldPos = zone.WorldPos;
                        ent.Position = new twin(Random.Range(-5, 5 + 1), Random.Range(-5, 5 + 1));
                        sessions.storyteller.Write(ent);
                    }
                }
        }

        void PushStoriesToSession(Session session)
        {
            if (sessionToTowerEntities.TryGetValue(session.address, out var ent))
            {
                if (this.worldData.towerZones.TryGetValue(ent.WorldPos, out var zone))
                {
                    link.Post(session.PeerId, ServeStory.op, new ServeStory(zone));
                }
                link.Post(session.PeerId, ServeStory.op, new ServeStory(ent));
            }
            PushEntitiesToSession(session); 
        }


        void PushEntitiesToSession(Session session)
        {
            if (sessionToTowerEntities.TryGetValue(session.address, out var ent))
            {
                if (this.worldData.towerZones.TryGetValue(ent.WorldPos, out var zone))
                {
                    foreach(var kvp in this.worldData.entitiesById)
                    {
                        if (kvp.Value.WorldPos == ent.WorldPos)
                        {
                            link.Post(session.PeerId, ServeStory.op, new ServeStory(kvp.Value));
                        }
                    }
                }
            }
        }

        public string CreateWorldBackup()
        {
            var sanitizedData = this.worldData.Clone();
            foreach (var sessionKVP in sessionToTowerEntities)
                sanitizedData.entitiesById.Remove(sessionKVP.Value.EntityId);

            var backupBytes = Capn.Crunchatize(sanitizedData);
            string backupName = "lastbackup";
            File.WriteAllBytes(Application.streamingAssetsPath + "/" + backupName, backupBytes);
            return backupName;
        }
        public void LoadFromWorldBackup(string backupName)
        {
            var backupBytes = File.ReadAllBytes(Application.streamingAssetsPath + "/" + backupName);
            this.worldData = Capn.Decrunchatize<WorldData>(backupBytes);
        }
    }


    [System.Serializable]
    struct WorldData
    {
        public Dictionary<twin, TowerZone> towerZones;
        public Dictionary<int, TowerEntity> entitiesById;
        public WorldData(bool generateNew)
        {
            if (generateNew == false) throw Dj.Crash("WorldData(false) is illegal");
            towerZones = new Dictionary<twin, TowerZone>();
            entitiesById = new Dictionary<int, TowerEntity>();
        }
        public WorldData Clone()
        {
            return (WorldData)MemberwiseClone();
        }
    }


    public class EntGenerator
    {
        int nextEntId = 0;
        TowerEntity NewEnt(string codename = "e")
        {
            int EntityId = nextEntId++;
            var ent = new TowerEntity(codename + "/" + EntityId);
            ent.EntityId = EntityId;
            ent.WorldPos = twin.zero;
            ent.Position = twin.zero;
            return ent;
        }
        public TowerEntity NewPlayerEntity()
        {
            var playerEnt = NewEnt("tpe");
            return playerEnt;
        }
        public TowerEntity NewRockEntity()
        {
            var rockEnt = NewEnt("rock");
            return rockEnt;
        }
    }

}