using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour
{
    public InputField InputField;
    public Button SendButton;
    public Text Messageboard;
    public int MaxMessages = 20;

    Queue<string> _messages = new Queue<string>();
    StringBuilder _builder = new StringBuilder();

    public event Action<string> Request_Send;

    // Start is called just before any of the Update methods is called the first time
    void Start()
    {
        SendButton.onClick.AddListener(SendInput);
    }

    void SendInput()
    {
        Request_Send?.Invoke(InputField.text);

        InputField.text = string.Empty;
        InputField.ActivateInputField();
    }

    public void AddToBoard(string sender, string message)
    {
        while (_messages.Count >= MaxMessages)
        {
            _messages.Dequeue();
        }

        _messages.Enqueue($"[{sender}]  {message}");

        foreach (var item in _messages)
        {
            _builder.AppendLine(item);
        }

        Messageboard.text = _builder.ToString();
        _builder.Clear();
    }
}