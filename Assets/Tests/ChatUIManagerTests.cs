using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    public class ChatUIManagerTests
    {
        InputField InputField;
        Button SendButton;
        Text Messageboard;
        ChatUIManager ChatUIManager;
        MessageTestDummy MessageTestDummy;

        [SetUp]
        public void SetUp()
        {
            InputField = MonoBehaviour.Instantiate(new GameObject()).AddComponent<InputField>();
            SendButton = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Button>();
            Messageboard = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Text>();
            ChatUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<ChatUIManager>();

            ChatUIManager.InputField = InputField;
            ChatUIManager.SendButton = SendButton;
            ChatUIManager.Messageboard = Messageboard;

            MessageTestDummy = new MessageTestDummy();

            ChatUIManager.Request_Send += MessageTestDummy.AcceptMessage;
        }

        [UnityTest]
        public IEnumerator TestSetUp_NoExceptions()
        {
            //SetUp()
            yield return null;
        }

        [UnityTest]
        public IEnumerator MessageInput_MessageDelivered()
        {
            yield return null;

            var originalMessage = "Test input";
            InputField.text = originalMessage;
            SendButton.onClick.Invoke();

            Assert.IsTrue(MessageTestDummy.MessageArrived);
            Assert.AreEqual(originalMessage, MessageTestDummy.LastMessage);
        }

        [UnityTest]
        public IEnumerator MessageInput_InputCleared()
        {
            yield return null;

            var originalMessage = "Test input";
            InputField.text = originalMessage;
            SendButton.onClick.Invoke();

            Assert.AreEqual(string.Empty, InputField.text);
        }

        [Test]
        public void AddToBoard_MessageAdded()
        {
            ChatUIManager.AddToBoard("TestUser", "Hellsing TV series OST needs to go vinyl");

            Assert.AreEqual("[TestUser]  Hellsing TV series OST needs to go vinyl\r\n", Messageboard.text);
        }

        [Test]
        public void AddToBoard_NumberMessages_CorrectOrder()
        {
            ChatUIManager.AddToBoard("admin", "0");
            ChatUIManager.AddToBoard("admin", "1");
            ChatUIManager.AddToBoard("admin", "2");
            ChatUIManager.AddToBoard("admin", "3");

            Assert.AreEqual("[admin]  0\r\n[admin]  1\r\n[admin]  2\r\n[admin]  3\r\n", Messageboard.text);
        }

        [Test]
        public void AddToBoard_NumberMessages_MessageLimit_CorrectOrderAndNumber()
        {
            ChatUIManager.MaxMessages = 3;

            ChatUIManager.AddToBoard("admin", "0");
            ChatUIManager.AddToBoard("admin", "1");
            ChatUIManager.AddToBoard("admin", "2");
            ChatUIManager.AddToBoard("admin", "3");

            Assert.AreEqual("[admin]  1\r\n[admin]  2\r\n[admin]  3\r\n", Messageboard.text);
        }
    }
}
