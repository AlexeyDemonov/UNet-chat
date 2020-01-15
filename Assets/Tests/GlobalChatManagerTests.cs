using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GlobalChatManagerTests
    {
        GlobalChatManager GlobalChatManager;
        MessageTestDummy MessageTestDummy;

        [SetUp]
        public void SetUp()
        {
            GlobalChatManager = new GlobalChatManager();
            MessageTestDummy = new MessageTestDummy();

            GlobalChatManager.Event_MessageArrived += MessageTestDummy.AcceptMessage;
        }

        [TearDown]
        public void TearDown()
        {
            GlobalChatManager.Event_MessageArrived -= MessageTestDummy.AcceptMessage;
            
            GlobalChatManager = null;
            MessageTestDummy = null;
        }

        [Test]
        public void BroadcastMessage_StringArgs_EventInvokes()
        {
            GlobalChatManager.BroadcastMessage("TestUser", "test");

            Assert.IsTrue(MessageTestDummy.MessageArrived);
            Assert.AreEqual("TestUser", MessageTestDummy.LastSender);
            Assert.AreEqual("test", MessageTestDummy.LastMessage);
        }

        //The rest can be tested only in network environment and after ChatClient class full implementation
    }
}
