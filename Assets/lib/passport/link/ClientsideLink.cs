namespace passport.link
{

using passport.crunch;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Barebones.MasterServer;
using Barebones.Networking;

public class ClientsideLink : MonoBehaviour {

////inspector properties?
	[Tooltip("Address to the server")]
	public string ServerIp = "127.0.0.1";

	[Tooltip("Port of the server")]
	public int ServerPort = 5000;

	[Header("Advanced ")]
	public float MinTimeToConnect = 0.5f;
	public float MaxTimeToConnect = 4f;
	public float TimeToConnect = 0.5f;
	public int MaxConnectTries = 3;

////public properties
	public bool IsConnected {get{return GetConnection().IsConnected;}}
	public IPeer Peer {get{return GetConnection().Peer;}}
	public bool ConnectionAttemptActive {get;private set;}
	public System.Action OnDisconnected {get;set;}

////MonoBehaviour

////public (Handler and Post functions)
	public void SetPostHandler(short opCode, IncommingMessageHandler handler) {
		Handlers[opCode] = new PacketHandler(opCode, handler);
	}
	public void SetPostHandler<BC>(short opCode, System.Action<BC> HandleBroadcast) where BC:struct {
		SetPostHandler(opCode, message=>{HandleBroadcast(Capn.Decrunchatize<BC>(message.AsBytes()));});
	}

	public IMessage Post(short opCode, object serializableObject, ResponseCallback responseCallback) {
		IMessage message = MessageHelper.Create(opCode, Capn.Crunchatize(serializableObject));
		if (IsConnected) {
			Peer.SendMessage(
				message,
				responseCallback,
				3,
				DeliveryMethod.Reliable
			);
		} else {
			Dj.Warn("Post to server failed (Not connected)");
			message.Status = ResponseStatus.NotConnected;
		}
		// Dj.Temp("Message status = "+message.Status);
		return message;
	}
	public IMessage Post<ReplyType>(short opCode, object serializableObject, System.Action<ReplyType> successCallback, System.Action<ResponseStatus> nonSuccessCallback = null) {
		return Post(opCode,serializableObject,(status,response)=>{
			if (status == ResponseStatus.Success) {
				successCallback(Capn.Decrunchatize<ReplyType>(response.AsBytes()));
			} else if (nonSuccessCallback != null) {
				nonSuccessCallback(status);
			}
		});
	}

	public virtual IClientSocket GetConnection()
	{
		return Msf.Connection;
	}
	public bool AttemptConnection(System.Action<bool> Callback, string serverIp = "", int serverPort = 0) {
		if (ConnectionAttemptActive) return false;
		//else//
		GetConnection().Disconnect();

		StartCoroutine(StartConnection(Callback, serverIp, serverPort));
		return true;
	}


////internal functions
	private IEnumerator StartConnection(System.Action<bool> Callback, string serverIp = "", int serverPort = 0)
	{

		if(serverIp!="") this.ServerIp = serverIp;
		if(serverPort>0) this.ServerPort = serverPort;

		ConnectionAttemptActive = true;
		
		// Wait a fraction of a second, in case we're also starting a master server?
		yield return new WaitForSeconds(0.2f);

		var connection = GetConnection();

		if (!connectionInitialized) {
			connectionInitialized = true;
			connection.Connected += Connected;
			connection.Disconnected += Disconnected;
		}

		for (int i = 0; i < MaxConnectTries; i++)
		{
			// Skip one frame
			yield return null;

			if (connection.IsConnected)
			{
				// If connected, wait a second before checking the status
				//yield return new WaitForSeconds(1);
				//continue;
				// yield break;
				break;
			}

			// If we got here, we're not connected 
			if (connection.IsConnecting)
			{
				Dj.Temp("Retrying to connect to server at: " + ServerIp + ":" + ServerPort);
			}
			else
			{
				Dj.Temp("Connecting to server at: " + ServerIp +":" + ServerPort);
			}

			connection.Connect(ServerIp, ServerPort);

			// Give a few seconds to try and connect
			yield return new WaitForSeconds(TimeToConnect);

			// If we're still not connected
			if (!connection.IsConnected)
			{
				TimeToConnect = Mathf.Min(TimeToConnect*2, MaxTimeToConnect);
			}
		}

		if (connection.IsConnected) {
			// try to log in
			Callback(true);
			Peer.MessageReceived += PeerOnMessageCallHandlers;
		} else {
			Dj.Temp("All connection attempts timed out");
			Callback(false);
		}
		ConnectionAttemptActive = false;
		
	}

	private void Disconnected()
	{
		TimeToConnect = MinTimeToConnect;
		if (this.OnDisconnected!=null)this.OnDisconnected();
	}

	private void Connected()
	{
		TimeToConnect = MinTimeToConnect;
		Dj.Temp("Connected to: " + ServerIp+":" + ServerPort);
	}

	void PeerOnMessageCallHandlers (IIncommingMessage message) {
		if (Handlers.ContainsKey(message.OpCode)) Handlers[message.OpCode].Handle(message);
		else Dj.Errorf("ClientsideLink rcv'd unhandled opcode {0}",message.OpCode);
	}

	Dictionary<short, PacketHandler> Handlers = new Dictionary<short, PacketHandler>();

	bool connectionInitialized = false;

	// void OnApplicationQuit()
	// {
	// 	var connection = GetConnection();

	// 	if (connection != null)
	// 		connection.Disconnect();
	// }
}
	
}