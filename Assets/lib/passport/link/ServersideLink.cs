namespace passport.link
{

using passport.crunch; // for serialization of data

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Barebones.MasterServer;
using Barebones.Networking;

[RequireComponent(typeof(MasterServerBehaviour))]
public class ServersideLink : MonoBehaviour {

////MonoBehaviour
	void Awake() {
		master = GetComponent<MasterServerBehaviour>();
		master.PeerConnected += this.PeerConnected;
		master.PeerDisconnected += this.PeerDisconnected;
	}
	void Start() {
		master.StartServer(); // start by default, obviously???
	}

////public: Post
	public IPeer GetPeer(int peerid) {
		return master.GetPeer(peerid);
	}
	public System.Action<IPeer> OnPeerConnect;
	public System.Action<IPeer> OnPeerDisconnect;
	public void SetPostHandler(short opCode, IncommingMessageHandler handler) {
		master.SetHandler(opCode, handler);
	}
	public void SetPostHandler<ACTION,REPLY>(short opCode, System.Action<PostAssistant<ACTION,REPLY>> PrepareAssistant) where ACTION:struct where REPLY:struct {
		master.SetHandler(opCode, (message)=>{
			var assistant = new PostAssistant<ACTION,REPLY>(message);
			PrepareAssistant(assistant);
			if (assistant.Done) {

			} else if (assistant.Delayed) {
				// if (assistant.responseStatus == ResponseStatus.Success) message.Respond(Capn.Crunchatize(assistant.reply), ResponseStatus.Success);
				// else message.Respond(assistant.responseStatus);
			} else {
				throw Dj.Crashf("generic action PrepareAssistant terminated illegally on opcode '{0}'!\nYou must call REPLY, REJECT, or PENDING before the action is done.", opCode);
			}
		});
	}

	public void Post(int peerId, short opCode, object serializableObject) {
		IPeer peer = master.GetPeer(peerId);
		if (peer == null) {
			Dj.Errorf("Post to peer#{0} failed", peerId);
		} else {
			peer.SendMessage(opCode, Capn.Crunchatize(serializableObject));
		}
	}
	// public void Post(int peerId, IMessage message, ResponseCallback responseCallback) {
	// 	IPeer peer = master.GetPeer(peerId);
	// 	if (peer == null) {
	// 		Dj.Errorf("Post to peer#{0} failed", peerId);
	// 	} else {
	// 		peer.SendMessage(
	// 			message,
	// 			responseCallback,
	// 			3,
	// 			DeliveryMethod.Reliable
	// 		);
	// 	}
	// }
	// public void Post(IEnumerable<int> peerIds, short opCode, object serializableObject, ResponseCallback responseCallback) {
	// 	IMessage message = MessageHelper.Create(opCode, Capn.Crunchatize(serializableObject));
	// 	foreach(int peerId in peerIds) {
	// 		Post(peerId, message, responseCallback);
	// 	}
	// }
	// public void Post(int peerId, short opCode, object serializableObject, ResponseCallback responseCallback) {
	// 	IMessage message = MessageHelper.Create(opCode, Capn.Crunchatize(serializableObject));
	// 	Post(peerId, message, responseCallback);
	// }

	void PeerConnected(IPeer peer) {
		if (this.OnPeerConnect != null)
		this.OnPeerConnect(peer);
	}
	void PeerDisconnected(IPeer peer) {
		if (this.OnPeerDisconnect != null)
		this.OnPeerDisconnect(peer);
	}

////internal properties
	MasterServerBehaviour master;
	
}
	
}