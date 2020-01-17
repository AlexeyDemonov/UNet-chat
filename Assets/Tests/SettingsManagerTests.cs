using System.IO;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SettingsManagerTests
    {
        readonly string _filename = "Settings.xml";
        SettingsManager SettingsManager;

        [SetUp]
        public void SetUp()
        {
            if(File.Exists(_filename))
                File.Delete(_filename);

            SettingsManager = new SettingsManager();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_filename))
                File.Delete(_filename);
        }

        [Test]
        public void Load_NoSettingsFile_ReturnsDefaultSettings()
        {
            var loadedContainer = SettingsManager.Load();

            Assert.AreEqual("user", loadedContainer.ClientName);
            Assert.AreEqual("localhost", loadedContainer.Address);
            Assert.AreEqual(7777, loadedContainer.Port);
        }

        [Test]
        public void Load_CorruptedSettingsFile_ReturnsDefaultSettings()
        {
            File.WriteAllText(_filename, "blablabla");

            var loadedContainer = SettingsManager.Load();

            Assert.AreEqual("user", loadedContainer.ClientName);
            Assert.AreEqual("localhost", loadedContainer.Address);
            Assert.AreEqual(7777, loadedContainer.Port);
        }

        [Test]
        public void Save_FileExists()
        {
            var originalName = "soldier76";
            var originalAddress = "185.60.112.157";
            var originalPort = 5060;

            SettingsContainer container = new SettingsContainer() { ClientName = originalName, Address = originalAddress, Port = originalPort};

            SettingsManager.Save(container);

            Assert.IsTrue(File.Exists(_filename));
        }

        [Test]
        public void Save_Load_EqualValues()
        {
            var originalName = "soldier76";
            var originalAddress = "185.60.112.157";
            var originalPort = 5060;

            var container = new SettingsContainer() { ClientName = originalName, Address = originalAddress, Port = originalPort };

            SettingsManager.Save(container);

            var loadedContainer = SettingsManager.Load();


            Assert.AreEqual(originalName, loadedContainer.ClientName);
            Assert.AreEqual(originalAddress, loadedContainer.Address);
            Assert.AreEqual(originalPort, loadedContainer.Port);
        }

        [UnityTest]
        public IEnumerator SaveAsync_Load_EqualValues()
        {
            var originalName = "soldier76";
            var originalAddress = "185.60.112.157";
            var originalPort = 5060;

            var container = new SettingsContainer() { ClientName = originalName, Address = originalAddress, Port = originalPort };

            var task = SettingsManager.SaveAsync(container);

            //task.Wait(); //Not supported: https://forum.unity.com/threads/support-for-async-await-in-tests.787853/

            while (!task.IsCompleted)
            {
                yield return null;
            }

            var loadedContainer = SettingsManager.Load();


            Assert.AreEqual(originalName, loadedContainer.ClientName);
            Assert.AreEqual(originalAddress, loadedContainer.Address);
            Assert.AreEqual(originalPort, loadedContainer.Port);
        }

        [Test]
        public void Save_Load_DeleteFile_SaveAgainSameValues_FileNotCreated()
        {
            var originalName = "soldier76";
            var originalAddress = "185.60.112.157";
            var originalPort = 5060;

            var container = new SettingsContainer() { ClientName = originalName, Address = originalAddress, Port = originalPort };

            SettingsManager.Save(container);
            SettingsManager.Load();
            File.Delete(_filename);
            SettingsManager.Save(container);

            Assert.IsTrue(!File.Exists(_filename));
        }

        [UnityTest]
        public IEnumerator SaveAsync_Load_DeleteFile_SaveAgainSameValues_FileNotCreated()
        {
            var originalName = "soldier76";
            var originalAddress = "185.60.112.157";
            var originalPort = 5060;

            var container = new SettingsContainer() { ClientName = originalName, Address = originalAddress, Port = originalPort };

            var task = SettingsManager.SaveAsync(container);
            while (!task.IsCompleted)
                yield return null;

            SettingsManager.Load();
            File.Delete(_filename);

            task = SettingsManager.SaveAsync(container);
            while (!task.IsCompleted)
                yield return null;

            Assert.IsTrue(!File.Exists(_filename));
        }

        [Test]
        public void Save_Load_DeleteFile_SaveNewValues_NewFileCreated()
        {
            for (int i = 0; i < 3; i++)
            {
                var originalName = "soldier76";
                var originalAddress = "185.60.112.157";
                var originalPort = 5060;

                var container = new SettingsContainer() { ClientName = originalName, Address = originalAddress, Port = originalPort };

                SettingsManager.Save(container);
                SettingsManager.Load();
                File.Delete(_filename);

                switch (i)
                {
                    case 0: container.ClientName = "Torbjorn"; break;
                    case 1: container.Address = "localhost"; break;
                    case 2: container.Port = 7080; break;
                }

                SettingsManager.Save(container);

                Assert.IsTrue(File.Exists(_filename));
                File.Delete(_filename);
            }
        }
    }
}
