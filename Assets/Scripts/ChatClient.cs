using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatClient : NetworkBehaviour
{
    enum Uniqueness { UNDEFINED, UNIQUE, NONUNIQUE } 

    public string ClientName;    
    Uniqueness NameIsUnique = Uniqueness.UNDEFINED;

    public event Action<ChatClient, string> Request_BroadcastMessage;
    public event Action<string, string> Event_MessageArrived;

    //public override void OnStartClient() //It is not working for some reason
    void Start()
    {
        if(this.isLocalPlayer)
        {
            StartCoroutine(Initialize());
        }
    }

    IEnumerator Initialize()
    {
        ClientName = PropertyBag.ClientName;
        CmdValidateNameUnique(ClientName);

        while (this.NameIsUnique == Uniqueness.UNDEFINED)
        {
            /*Do nothing*/
            yield return null;//Wait for confirmation
        }

        if (this.NameIsUnique == Uniqueness.UNIQUE)
        {
            PropertyBag.LocalChatUIManager.Request_Send += Handle_LocalSendRequest;
            Event_MessageArrived += PropertyBag.LocalChatUIManager.AddToBoard;

            CmdConnectToGlobalManager();
        }
        else if (this.NameIsUnique == Uniqueness.NONUNIQUE)
        {
            PropertyBag.ErrorMessage = "Client with this name already exists on this server";
            CmdDisconnectClient();
        }
    }

    [Command]
    void CmdValidateNameUnique(string name)
    {
        this.ClientName = name;//Server side synchronisation
        this.NameIsUnique = PropertyBag.GlobalChatManager.ValidateNameIsNew(this.ClientName) ? Uniqueness.UNIQUE : Uniqueness.NONUNIQUE;

        TargetSetNameUniquness(this.connectionToClient, this.NameIsUnique);
    }

    [TargetRpc]
    void TargetSetNameUniquness(NetworkConnection connection, Uniqueness uniqueness)
    {
        this.NameIsUnique = uniqueness;//Client side synchronisation
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