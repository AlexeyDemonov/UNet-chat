using System;
using UnityEngine;
using UnityEngine.Networking;

public class ChatClient : NetworkBehaviour
{
    [SyncVar]
    public string ClientName;

    public override void OnStartClient()
    {
        //TODO
    }

    public void Send(string message)
    {
        //TODO
        Request_BroadcastMessage?.Invoke(this, message);
    }

    public event Action<ChatClient, string> Request_BroadcastMessage;
    public event Action<string, string> Event_MessageArrived;

    [TargetRpc]
    public void TargetAcceptMessage(NetworkConnection connection, string sender, string message)
    {
        //TODO
    }
}