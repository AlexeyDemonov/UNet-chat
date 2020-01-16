using System;
using UnityEngine;

public class SettingsTestDummy
{
    public readonly string TestName = "soldier76";
    public readonly string TestAddress = "185.60.112.157";
    public readonly int TestPort = 5060;

    public bool ContainerRequested { get; private set; }
    public bool ContainerAccepted { get; private set; }
    public SettingsContainer AcceptedContainer { get; private set; }

    public SettingsContainer GiveContainer()
    {
        ContainerRequested = true;
        return new SettingsContainer() { ClientName = TestName, Address = TestAddress, Port = TestPort };
    }

    public void AcceptContainer (SettingsContainer container)
    {
        ContainerAccepted = true;
        AcceptedContainer = container;
    }
}