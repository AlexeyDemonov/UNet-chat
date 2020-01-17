using System;
using UnityEngine;
using UnityEngine.Networking;

public class ChatClient : NetworkBehaviour
{
    enum Uniqueness { UNDEFINED, UNIQUE, NONUNIQUE } 

    [SyncVar]
    public string ClientName;
    [SyncVar]
    Uniqueness NameIsUnique = Uniqueness.UNDEFINED;

    public event Action<ChatClient, string> Request_BroadcastMessage;
    public event Action<string, string> Event_MessageArrived;

    public override void OnStartClient()
    {
        if(this.isLocalPlayer)
        {
            ClientName = PropertyBag.ClientName;
            CmdValidateNameUnique();

            if(this.NameIsUnique == Uniqueness.UNDEFINED)
                Debug.LogError("ChatClient.OnStartClient: NameIsUnique is undefined on client side");
            else if(this.NameIsUnique == Uniqueness.UNIQUE)
            {
                PropertyBag.LocalChatUIManager.Request_Send += Handle_LocalSendRequest;
                Event_MessageArrived += PropertyBag.LocalChatUIManager.AddToBoard;

                CmdConnectToGlobalManager();
            }
            else/*if(this.NameIsUnique == Uniqueness.NONUNIQUE)*/
            {
                PropertyBag.ErrorMessage = "Client with this name already exists on this server";
                CmdDisconnectClient();
            }
        }
    }

    [Command]
    void CmdValidateNameUnique()
    {
        if(this.ClientName/*on server side*/ == null)
            Debug.LogError("ChatClient.CmdValidateNameUnique: ClientName is null on server side");

        this.NameIsUnique = PropertyBag.GlobalChatManager.ValidateNameIsNew(this.ClientName) ? Uniqueness.UNIQUE : Uniqueness.NONUNIQUE;
    }

    [Command]
    void CmdDisconnectClient()
    {
        this.connectionToClient.Disconnect();
        this.connectionToClient.Dispose();
    }

    [Command]
    void CmdConnectToGlobalManager()
    {
        PropertyBag.GlobalChatManager.Handle_Connection(this);
    }

    public void Handle_LocalSendRequest(string message)
    {
        Event_MessageArrived?.Invoke(this.ClientName, message);
        CmdSendMessageToServer(message);
    }

    [Command]
    void CmdSendMessageToServer(string message)
    {
        Request_BroadcastMessage?.Invoke(this, message);
    }

    [TargetRpc]
    public void TargetAcceptMessage(NetworkConnection connection, string sender, string message)
    {
        Event_MessageArrived?.Invoke(sender, message);
    }
}