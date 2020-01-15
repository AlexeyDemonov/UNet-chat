using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ChatGameManagerTests
    {
        ChatGameManager ChatGameManager;
        MessageTestDummy MessageTestDummy;

        [SetUp]
        public void SetUp()
        {
            ChatGameManager = new ChatGameManager();
            MessageTestDummy = new MessageTestDummy();

            ChatGameManager.Request_BroadcastMessage += MessageTestDummy.AcceptMessage;
        }

        [TearDown]
        public void TearDown()
        {
            ChatGameManager.Request_BroadcastMessage -= MessageTestDummy.AcceptMessage;

            ChatGameManager = null;
            MessageTestDummy = null;
        }

        [Test]
        public void MessageArrived_RandomMessage_NoReaction()
        {
            ChatGameManager.MessageArrived("Boris", "Heavy is good");

            Assert.IsFalse(MessageTestDummy.MessageArrived);
            Assert.IsNull(MessageTestDummy.LastMessage);
            Assert.IsNull(MessageTestDummy.LastSender);
        }

        [Test]
        public void MessageArrived_MessageFromServer_NoReaction()
        {
            ChatGameManager.MessageArrived("Server", "!game");

            Assert.IsFalse(MessageTestDummy.MessageArrived);
            Assert.IsNull(MessageTestDummy.LastMessage);
            Assert.IsNull(MessageTestDummy.LastSender);
        }

        [Test]
        public void MessageArrived_GameMessage_GameStarts()
        {
            ChatGameManager.MessageArrived("Boris", "!game");

            Assert.IsTrue(MessageTestDummy.MessageArrived);
            Assert.IsTrue(MessageTestDummy.LastMessage.StartsWith("!GAME! Boris started a game"));
            Assert.AreEqual("Server", MessageTestDummy.LastSender);
        }

        [Test]
        public void MessageArrived_DuplicatedGameMessage_NoReaction()
        {
            ChatGameManager.MessageArrived("Boris", "!game");
            ChatGameManager.MessageArrived("Tommy", "!game");
            ChatGameManager.MessageArrived("Avi", "!game");

            Assert.IsTrue(MessageTestDummy.LastMessage.StartsWith("!GAME! Boris started a game"));
        }

        [Test]
        public void MessageArrived_RandomMessageAfterGameMessage_NoReaction()
        {
            ChatGameManager.MessageArrived("Boris", "!game");
            ChatGameManager.MessageArrived("Avi", "Look in the dog");

            Assert.IsTrue(MessageTestDummy.LastMessage.StartsWith("!GAME! Boris started a game"));
        }

        [Test]
        public void MessageArrived_NumberMessageAfterGameMessage_GameReaction()
        {
            ChatGameManager.MessageArrived("Boris", "!game");
            ChatGameManager.MessageArrived("Tommy", "50");

            Assert.IsTrue
                (
                MessageTestDummy.LastMessage == "Greater" ||
                MessageTestDummy.LastMessage == "Less" ||
                MessageTestDummy.LastMessage.StartsWith("!WINNER! Tommy won the game")
                );
        }

        [Test]
        public void MessageArrived_NumberMessage_GameMayBeWon()
        {
            ChatGameManager.MessageArrived("Boris", "!game");

            for (int i = 0; i < 101; i++)
            {
                ChatGameManager.MessageArrived("Tommy", i.ToString());

                if(MessageTestDummy.LastMessage.StartsWith("!WINNER! Tommy won the game"))
                    break;
            }

            Assert.IsTrue(MessageTestDummy.LastMessage.StartsWith("!WINNER! Tommy won the game"));
        }

        [Test]
        public void MessageArrived_RandomMessageAfterWin_NoReaction()
        {
            ChatGameManager.MessageArrived("Boris", "!game");

            for (int i = 0; i < 101; i++)
            {
                ChatGameManager.MessageArrived("Tommy", i.ToString());

                if (MessageTestDummy.LastMessage.StartsWith("!WINNER! Tommy won the game"))
                    break;
            }

            ChatGameManager.MessageArrived("Bullet Tooth Tony", "Avi, pull your socks up");

            Assert.IsTrue(MessageTestDummy.LastMessage.StartsWith("!WINNER! Tommy won the game"));
        }

        [Test]
        public void MessageArrived_GameMessageAfterWin_GameRestarts()
        {
            ChatGameManager.MessageArrived("Boris", "!game");

            for (int i = 0; i < 101; i++)
            {
                ChatGameManager.MessageArrived("Tommy", i.ToString());

                if (MessageTestDummy.LastMessage.StartsWith("!WINNER! Tommy won the game"))
                    break;
            }

            ChatGameManager.MessageArrived("Brick Top", "!game");

            Assert.IsTrue(MessageTestDummy.LastMessage.StartsWith("!GAME! Brick Top started a game"));
        }
    }
}
