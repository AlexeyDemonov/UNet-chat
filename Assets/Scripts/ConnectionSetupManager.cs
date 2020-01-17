using System;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionSetupManager : NetworkManager
{
    public event Action<NetworkConnection> Event_ClientDisconnected;

    public void Join(SettingsContainer settings)
    {
        InitializeSettings(settings);
        base.StartClient();
    }

    public void Host(SettingsContainer settings)
    {
        InitializeSettings(settings);

        var gameManager = new ChatGameManager();
        var globalChatManager = new GlobalChatManager();

        gameManager.Request_BroadcastMessage += globalChatManager.BroadcastMessage;
        globalChatManager.Event_MessageArrived += gameManager.MessageArrived;
        this.Event_ClientDisconnected += globalChatManager.Handle_Disconnection;

        PropertyBag.ChatGameManager = gameManager;
        PropertyBag.GlobalChatManager = globalChatManager;

        base.StartHost();
    }

    void InitializeSettings(SettingsContainer settings)
    {
        PropertyBag.ClientName = settings.ClientName;
        base.networkAddress = settings.Address;
        base.networkPort = settings.Port;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Event_ClientDisconnected?.Invoke(conn);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode) => ReportConnectionError();
    public override void OnServerError(NetworkConnection conn, int errorCode) => ReportConnectionError();
    public override void OnClientDisconnect(NetworkConnection conn) => ReportConnectionError();

    void ReportConnectionError()
    {
        if(PropertyBag.ErrorMessage == null)//Do not override first error
            PropertyBag.ErrorMessage = "Connection error, check your address/port settings";
    }
}