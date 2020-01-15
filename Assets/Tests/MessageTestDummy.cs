using System;
using UnityEngine;

public class MessageTestDummy
{
    public string LastSender { get; set; }
    public string LastMessage { get; set; }
    public bool MessageArrived { get; set; }

    public void AcceptMessage(string sender, string message)
    {
        LastSender = sender;
        LastMessage = message;
        MessageArrived = true;
    }

    public void AcceptMessage(string message)
    {
        AcceptMessage(null, message);
    }
}