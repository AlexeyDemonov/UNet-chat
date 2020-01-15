using System;
using UnityEngine;

public class ChatGameManager
{
    readonly string _senderName = "Server";

    bool _gameIsRunning;
    int _pickedNumber;

    public event Action<string, string> Request_BroadcastMessage;

    public void MessageArrived(string sender, string message)
    {
        if(sender == this._senderName)
            return;

        if(!_gameIsRunning)
        {
            if(message == "!game")
            {
                StartGame(sender);
            }
        }
        else
        {
            if(int.TryParse(message, out int number))
            {
                CheckNumber(sender, number);
            }
        }
    }

    void StartGame(string sender)
    {
        _gameIsRunning = true;
        _pickedNumber = UnityEngine.Random.Range(0, 101);

        string message = $"!GAME! {sender} started a game! I picked a number in range 0-100, try to guess it";
        Request_BroadcastMessage?.Invoke(_senderName, message);
    }

    private void CheckNumber(string sender, int number)
    {
        if(number < 0 || number > 100)
        {
            /*Ignore*/
            return;
        }

        string message = null;

        if(number > _pickedNumber)
            message = "Greater";
        else if (number < _pickedNumber)
            message = "Less";
        else/*if(number == _pickedNumber)*/
        {
            message = $"!WINNER! {sender} won the game! It is indeed {_pickedNumber}";
            _gameIsRunning = false;
        }

        Request_BroadcastMessage?.Invoke(_senderName, message);
    }
}