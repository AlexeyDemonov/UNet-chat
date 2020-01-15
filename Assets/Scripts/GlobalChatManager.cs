using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalChatManager
{
    //=============================================================================
    //Fields
    readonly string _managerName = "Server";
    HashSet<string> _names = new HashSet<string>();
    Dictionary<int, ChatClient> _clients = new Dictionary<int, ChatClient>();

    //=============================================================================
    //Events
    public event Action<string,string> Event_MessageArrived;

    //=============================================================================
    //Methods
    public void BroadcastMessage(string sender, string message)
    {
        foreach (var client in _clients.Values)
        {
            var connection = client.connectionToClient;
            client.TargetAcceptMessage( connection, sender, message );
        }

        Event_MessageArrived?.Invoke(sender, message);
    }

    public void BroadcastMessage(ChatClient sender, string message)
    {
        List<int> keys = new List<int>(_clients.Keys);
        int senderKey = sender.connectionToClient.connectionId;
        keys.Remove(senderKey);

        foreach (var key in keys)
        {
            var recipient = _clients[key];
            var connection = recipient.connectionToClient;
            recipient.TargetAcceptMessage( connection, sender.ClientName, message );
        }

        Event_MessageArrived?.Invoke(sender.ClientName, message);
    }

    public bool ValidateNameIsNew(string name)
    {
        return !_names.Contains(name);
    }

    public void Handle_Connection(ChatClient newClient)
    {
        var name = newClient.ClientName;
        var id = newClient.connectionToClient.connectionId;

        if(_names.Contains(name))
            BroadcastMessage(_managerName, $"!Error! {name} already exists");
        if (_clients.ContainsKey(id))
            BroadcastMessage(_managerName, $"!Error! connection {id} already exists");

        _names.Add(name);
        _clients[id] = newClient;

        newClient.Request_BroadcastMessage += BroadcastMessage;

        BroadcastMessage(_managerName, $"{name} joined the chat");
    }

    public void Handle_Disconnection(NetworkConnection connection)
    {
        var id = connection.connectionId;

        if(_clients.ContainsKey(id))
        {
            var client = _clients[id];
            var name = client.ClientName;

            client.Request_BroadcastMessage -= BroadcastMessage;
            _names.Remove(name);
            _clients.Remove(id);

            BroadcastMessage(_managerName, $"{name} left the chat");
        }
    }
}