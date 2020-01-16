using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    public class LogonUIManagerTests
    {   
        //===============================================================================================================
        //Build
        [UnityTest]
        public IEnumerator LogonUIManagerBuild_FieldsEmpty_NoException()
        {
            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            yield return null;//skip frame
        }

        //===============================================================================================================
        //Settings
        [UnityTest]
        public IEnumerator Start_SettingsLoad_SettingsAppearInFields()
        {
            var nameInput = MonoBehaviour.Instantiate(new GameObject()).AddComponent<InputField>();
            var joinAddressInput = MonoBehaviour.Instantiate(new GameObject()).AddComponent<InputField>();
            var joinPortInput = MonoBehaviour.Instantiate(new GameObject()).AddComponent<InputField>();
            var hostPortInput = MonoBehaviour.Instantiate(new GameObject()).AddComponent<InputField>();

            var SettingsTestDummy = new SettingsTestDummy();

            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            LogonUIManager.NameInput = nameInput;
            LogonUIManager.JoinAddressInput = joinAddressInput;
            LogonUIManager.JoinPortInput = joinPortInput;
            LogonUIManager.HostPortInput = hostPortInput;

            LogonUIManager.Request_LoadSettings += SettingsTestDummy.GiveContainer;

            yield return null;//skip frame

            Assert.IsTrue(SettingsTestDummy.ContainerRequested);
            Assert.AreEqual(SettingsTestDummy.TestName, nameInput.text);
            Assert.AreEqual(SettingsTestDummy.TestAddress, joinAddressInput.text);
            var expectedPort = SettingsTestDummy.TestPort.ToString();
            Assert.AreEqual(expectedPort, joinPortInput.text);
            Assert.AreEqual(expectedPort, hostPortInput.text);
        }

        //===============================================================================================================
        //ErrorAcceptButton
        [UnityTest]
        public IEnumerator ErrorAcceptButton_Click_ErrorUIDeactivated_NameUIActivated()
        {
            GameObject errorUI = MonoBehaviour.Instantiate(new GameObject());
            GameObject nameUI = MonoBehaviour.Instantiate(new GameObject());

            errorUI.SetActive(true);
            nameUI.SetActive(false);

            Button errorAcceptButton = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Button>();

            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            LogonUIManager.ErrorUI = errorUI;
            LogonUIManager.NameInputUI = nameUI;
            LogonUIManager.ErrorAcceptButton = errorAcceptButton;

            yield return null;//skip frame

            errorAcceptButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(!errorUI.activeSelf);
            Assert.IsTrue(nameUI.activeSelf);
        }

        //===============================================================================================================
        //NameInput
        void SetUp_NameInput(out GameObject nameUI, out GameObject modeChoiceUI, out InputField nameInput, out Image nameInputImage, out Button nameConfirmButton)
        {
            nameUI = MonoBehaviour.Instantiate(new GameObject());
            modeChoiceUI = MonoBehaviour.Instantiate(new GameObject());

            nameUI.SetActive(true);
            modeChoiceUI.SetActive(false);

            GameObject nameInputObject = MonoBehaviour.Instantiate(new GameObject());
            nameInput = nameInputObject.AddComponent<InputField>();
            nameInputImage = nameInputObject.AddComponent<Image>();

            nameConfirmButton = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Button>();

            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            LogonUIManager.NameInputUI = nameUI;
            LogonUIManager.ModeChoiceUI = modeChoiceUI;
            LogonUIManager.NameInput = nameInput;
            LogonUIManager.NameConfirmButton = nameConfirmButton;
        }

        [UnityTest]
        public IEnumerator NameConfirmButton_Click_EmptyNameField_InputFieldRed_UIStateUnchanged()
        {
            SetUp_NameInput(out GameObject nameUI, out GameObject modeChoiceUI, out InputField nameInput, out Image nameInputImage, out Button nameConfirmButton);

            yield return null;//skip frame

            nameConfirmButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(nameUI.activeSelf);
            Assert.IsTrue(!modeChoiceUI.activeSelf);
            Assert.AreEqual(Color.red, nameInputImage.color);
        }

        [UnityTest]
        public IEnumerator NameConfirmButton_Click_ShortName_InputFieldRed_UIStateUnchanged()
        {
            SetUp_NameInput(out GameObject nameUI, out GameObject modeChoiceUI, out InputField nameInput, out Image nameInputImage, out Button nameConfirmButton);

            yield return null;//skip frame

            nameInput.text = "Me";
            nameConfirmButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(nameUI.activeSelf);
            Assert.IsTrue(!modeChoiceUI.activeSelf);
            Assert.AreEqual(Color.red, nameInputImage.color);
        }

        [UnityTest]
        public IEnumerator NameConfirmButton_Click_LongName_InputFieldRed_UIStateUnchanged()
        {
            SetUp_NameInput(out GameObject nameUI, out GameObject modeChoiceUI, out InputField nameInput, out Image nameInputImage, out Button nameConfirmButton);

            yield return null;//skip frame

            nameInput.text = "Honorificabilitudinitatibus";
            nameConfirmButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(nameUI.activeSelf);
            Assert.IsTrue(!modeChoiceUI.activeSelf);
            Assert.AreEqual(Color.red, nameInputImage.color);
        }

        [UnityTest]
        public IEnumerator NameConfirmButton_Click_ServerName_InputFieldRed_UIStateUnchanged()
        {
            SetUp_NameInput(out GameObject nameUI, out GameObject modeChoiceUI, out InputField nameInput, out Image nameInputImage, out Button nameConfirmButton);

            yield return null;//skip frame

            nameInput.text = "SerVer";
            nameConfirmButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(nameUI.activeSelf);
            Assert.IsTrue(!modeChoiceUI.activeSelf);
            Assert.AreEqual(Color.red, nameInputImage.color);
        }

        [UnityTest]
        public IEnumerator NameConfirmButton_Click_AcceptableName_InputFieldGreen_UIStateChanged()
        {
            SetUp_NameInput(out GameObject nameUI, out GameObject modeChoiceUI, out InputField nameInput, out Image nameInputImage, out Button nameConfirmButton);

            yield return null;//skip frame

            nameInput.text = "Torbjorn";
            nameConfirmButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(!nameUI.activeSelf);
            Assert.IsTrue(modeChoiceUI.activeSelf);
            Assert.AreEqual(Color.green, nameInputImage.color);
        }

        //===============================================================================================================
        //ModeChoice
        [UnityTest]
        public IEnumerator JoinModeButton_Click_ModeChoiceUIDeactivated_JoinUIActivated()
        {
            GameObject modeChoiceUI = MonoBehaviour.Instantiate(new GameObject());
            GameObject joinUI = MonoBehaviour.Instantiate(new GameObject());

            modeChoiceUI.SetActive(true);
            joinUI.SetActive(false);

            Button joinModeButton = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Button>();

            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            LogonUIManager.ModeChoiceUI = modeChoiceUI;
            LogonUIManager.JoinModeUI = joinUI;
            LogonUIManager.JoinModeButton = joinModeButton;

            yield return null;//skip frame

            joinModeButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(!modeChoiceUI.activeSelf);
            Assert.IsTrue(joinUI.activeSelf);
        }

        [UnityTest]
        public IEnumerator HostModeButton_Click_ModeChoiceUIDeactivated_HostUIActivated()
        {
            GameObject modeChoiceUI = MonoBehaviour.Instantiate(new GameObject());
            GameObject hostUI = MonoBehaviour.Instantiate(new GameObject());

            modeChoiceUI.SetActive(true);
            hostUI.SetActive(false);

            Button hostModeButton = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Button>();

            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            LogonUIManager.ModeChoiceUI = modeChoiceUI;
            LogonUIManager.HostModeUI = hostUI;
            LogonUIManager.HostModeButton = hostModeButton;

            yield return null;//skip frame

            hostModeButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.IsTrue(!modeChoiceUI.activeSelf);
            Assert.IsTrue(hostUI.activeSelf);
        }

        //===============================================================================================================
        //Join
        void SetUp_JoinInput(out GameObject joinUI, out GameObject loadingUI,
            out InputField addressInput, out Image addressInputImage,
            out InputField portInput, out Image portInputImage,
            out SettingsTestDummy settingsAccepter, out SettingsTestDummy joinAccepter,
            out Button joinButton)
        {
            joinUI = MonoBehaviour.Instantiate(new GameObject());
            loadingUI = MonoBehaviour.Instantiate(new GameObject());

            joinUI.SetActive(true);
            loadingUI.SetActive(false);

            InputField nameInput = MonoBehaviour.Instantiate(new GameObject()).AddComponent<InputField>();
            nameInput.text = "TestUser";

            GameObject addressInputObject = MonoBehaviour.Instantiate(new GameObject());
            addressInput = addressInputObject.AddComponent<InputField>();
            addressInputImage = addressInputObject.AddComponent<Image>();

            GameObject portInputObject = MonoBehaviour.Instantiate(new GameObject());
            portInput = portInputObject.AddComponent<InputField>();
            portInputImage = portInputObject.AddComponent<Image>();

            joinButton = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Button>();

            settingsAccepter = new SettingsTestDummy();
            joinAccepter = new SettingsTestDummy();

            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            LogonUIManager.NameInput = nameInput;
            LogonUIManager.JoinModeUI = joinUI;
            LogonUIManager.LoadingUI = loadingUI;
            LogonUIManager.JoinAddressInput = addressInput;
            LogonUIManager.JoinPortInput = portInput;
            LogonUIManager.JoinButton = joinButton;
            LogonUIManager.Request_SaveSettings += settingsAccepter.AcceptContainer;
            LogonUIManager.Request_Join += joinAccepter.AcceptContainer;
        }

        [UnityTest]
        public IEnumerator JoinButton_Click_AddressIncorrect_PortIncorrect_AddressInputRed_PortInputRed_UiUnchanged_EventsNotInvoked()
        {
            string[] addresses ={string.Empty, "laclahost", "asdasdf", "192.168.100", "192.168.200.40.80", "192,168,0,50", "192.402.100.24", "103.50.-20.40", "103.50.-20.40", "103.50.-20.40"};
            string[] ports =    {string.Empty, "asdasdf",   "10a3",    "0",           "1",                 "-50",          "1000",           "1023",          "49152",         "49153"};
            int lenght = addresses.Length;

            for (int i = 0; i < lenght; i++)
            {
                SetUp_JoinInput(out GameObject joinUI, out GameObject loadingUI,
                    out InputField addressInput, out Image addressInputImage,
                    out InputField portInput, out Image portInputImage,
                    out SettingsTestDummy settingsAccepter, out SettingsTestDummy joinAccepter,
                    out Button joinButton);

                yield return null;//skip frame

                addressInput.text = addresses[i];
                portInput.text = ports[i];

                yield return null;//skip frame

                joinButton.onClick.Invoke();

                yield return null;//skip frame

                Assert.AreEqual(Color.red, addressInputImage.color);
                Assert.AreEqual(Color.red, portInputImage.color);

                Assert.IsTrue(joinUI.activeSelf);
                Assert.IsTrue(!loadingUI.activeSelf);

                Assert.IsTrue(!settingsAccepter.ContainerAccepted);
                Assert.IsNull(settingsAccepter.AcceptedContainer);

                Assert.IsTrue(!joinAccepter.ContainerAccepted);
                Assert.IsNull(joinAccepter.AcceptedContainer);
            }

        }

        [UnityTest]
        public IEnumerator JoinButton_Click_AddressIncorrect_PortCorrect_AddressInputRed_PortInputGreen_UiUnchanged_EventsNotInvoked()
        {
            string[] addresses = { string.Empty, "laclahost", "asdasdf", "192.168.100", "192.168.200.40.80", "192,168,0,50", "192.402.100.24", "103.50.-20.40", "103.50.-20.40", "103.50.-20.40" };
            string port = "7777";
            int lenght = addresses.Length;

            for (int i = 0; i < lenght; i++)
            {
                SetUp_JoinInput(out GameObject joinUI, out GameObject loadingUI,
                    out InputField addressInput, out Image addressInputImage,
                    out InputField portInput, out Image portInputImage,
                    out SettingsTestDummy settingsAccepter, out SettingsTestDummy joinAccepter,
                    out Button joinButton);

                yield return null;//skip frame

                addressInput.text = addresses[i];
                portInput.text = port;

                yield return null;//skip frame

                joinButton.onClick.Invoke();

                yield return null;//skip frame

                Assert.AreEqual(Color.red, addressInputImage.color);
                Assert.AreEqual(Color.green, portInputImage.color);

                Assert.IsTrue(joinUI.activeSelf);
                Assert.IsTrue(!loadingUI.activeSelf);

                Assert.IsTrue(!settingsAccepter.ContainerAccepted);
                Assert.IsNull(settingsAccepter.AcceptedContainer);

                Assert.IsTrue(!joinAccepter.ContainerAccepted);
                Assert.IsNull(joinAccepter.AcceptedContainer);
            }

        }

        [UnityTest]
        public IEnumerator JoinButton_Click_AddressCorrect_PortIncorrect_AddressInputGreen_PortInputRed_UiUnchanged_EventsNotInvoked()
        {
            string address = "localhost";
            string[] ports = { string.Empty, "asdasdf", "10a3", "0", "1", "-50", "1000", "1023", "49152", "49153" };
            int lenght = ports.Length;

            for (int i = 0; i < lenght; i++)
            {
                SetUp_JoinInput(out GameObject joinUI, out GameObject loadingUI,
                    out InputField addressInput, out Image addressInputImage,
                    out InputField portInput, out Image portInputImage,
                    out SettingsTestDummy settingsAccepter, out SettingsTestDummy joinAccepter,
                    out Button joinButton);

                yield return null;//skip frame

                addressInput.text = address;
                portInput.text = ports[i];

                yield return null;//skip frame

                joinButton.onClick.Invoke();

                yield return null;//skip frame

                Assert.AreEqual(Color.green, addressInputImage.color);
                Assert.AreEqual(Color.red, portInputImage.color);

                Assert.IsTrue(joinUI.activeSelf);
                Assert.IsTrue(!loadingUI.activeSelf);

                Assert.IsTrue(!settingsAccepter.ContainerAccepted);
                Assert.IsNull(settingsAccepter.AcceptedContainer);

                Assert.IsTrue(!joinAccepter.ContainerAccepted);
                Assert.IsNull(joinAccepter.AcceptedContainer);
            }

        }

        [UnityTest]
        public IEnumerator JoinButton_Click_AddressCorrect_PortCorrect_AddressInputGreen_PortInputGreen_UiChanged_EventsInvoked_SettingsSent()
        {
            string[] addresses = { "localhost", "192.168.0.40" };
            string[] ports = { "1040", "7777" };
            int lenght = addresses.Length;

            for (int i = 0; i < lenght; i++)
            {
                SetUp_JoinInput(out GameObject joinUI, out GameObject loadingUI,
                    out InputField addressInput, out Image addressInputImage,
                    out InputField portInput, out Image portInputImage,
                    out SettingsTestDummy settingsAccepter, out SettingsTestDummy joinAccepter,
                    out Button joinButton);

                yield return null;//skip frame

                addressInput.text = addresses[i];
                portInput.text = ports[i];

                yield return null;//skip frame

                joinButton.onClick.Invoke();

                yield return null;//skip frame

                Assert.AreEqual(Color.green, addressInputImage.color);
                Assert.AreEqual(Color.green, portInputImage.color);

                Assert.IsTrue(!joinUI.activeSelf);
                Assert.IsTrue(loadingUI.activeSelf);

                Assert.IsTrue(settingsAccepter.ContainerAccepted);

                var settingsContainer = settingsAccepter.AcceptedContainer;
                Assert.IsTrue(settingsContainer != null);
                Assert.AreEqual("TestUser", settingsContainer.ClientName);
                Assert.AreEqual(addresses[i], settingsContainer.Address);
                Assert.AreEqual(ports[i], settingsContainer.Port.ToString());

                Assert.IsTrue(joinAccepter.ContainerAccepted);

                var joinContainer = joinAccepter.AcceptedContainer;
                Assert.IsTrue(joinContainer != null);
                Assert.AreEqual("TestUser", joinContainer.ClientName);
                Assert.AreEqual(addresses[i], joinContainer.Address);
                Assert.AreEqual(ports[i], joinContainer.Port.ToString());
            }

        }

        //===============================================================================================================
        //Host
        void SetUp_HostInput(out GameObject hostUI, out GameObject loadingUI,
            out InputField portInput, out Image portInputImage,
            out SettingsTestDummy settingsAccepter, out SettingsTestDummy hostAccepter,
            out Button hostButton)
        {
            hostUI = MonoBehaviour.Instantiate(new GameObject());
            loadingUI = MonoBehaviour.Instantiate(new GameObject());

            hostUI.SetActive(true);
            loadingUI.SetActive(false);

            InputField nameInput = MonoBehaviour.Instantiate(new GameObject()).AddComponent<InputField>();
            nameInput.text = "TestUser";

            GameObject portInputObject = MonoBehaviour.Instantiate(new GameObject());
            portInput = portInputObject.AddComponent<InputField>();
            portInputImage = portInputObject.AddComponent<Image>();

            hostButton = MonoBehaviour.Instantiate(new GameObject()).AddComponent<Button>();

            settingsAccepter = new SettingsTestDummy();
            hostAccepter = new SettingsTestDummy();

            var LogonUIManager = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LogonUIManager>();
            LogonUIManager.NameInput = nameInput;
            LogonUIManager.HostModeUI = hostUI;
            LogonUIManager.LoadingUI = loadingUI;
            LogonUIManager.HostPortInput = portInput;
            LogonUIManager.HostButton = hostButton;
            LogonUIManager.Request_SaveSettings += settingsAccepter.AcceptContainer;
            LogonUIManager.Request_Host += hostAccepter.AcceptContainer;
        }

        [UnityTest]
        public IEnumerator HostButton_Click_PortIncorrect_PortInputRed_UiUnchanged_EventsNotInvoked()
        {
            string[] ports = { string.Empty, "asdasdf", "10a3", "0", "1", "-50", "1000", "1023", "49152", "49153" };
            int lenght = ports.Length;

            for (int i = 0; i < lenght; i++)
            {
                SetUp_HostInput(out GameObject hostUI, out GameObject loadingUI,
                    out InputField portInput, out Image portInputImage,
                    out SettingsTestDummy settingsAccepter, out SettingsTestDummy joinAccepter,
                    out Button hostButton);

                yield return null;//skip frame

                portInput.text = ports[i];

                yield return null;//skip frame

                hostButton.onClick.Invoke();

                yield return null;//skip frame

                Assert.AreEqual(Color.red, portInputImage.color);

                Assert.IsTrue(hostUI.activeSelf);
                Assert.IsTrue(!loadingUI.activeSelf);

                Assert.IsTrue(!settingsAccepter.ContainerAccepted);
                Assert.IsNull(settingsAccepter.AcceptedContainer);

                Assert.IsTrue(!joinAccepter.ContainerAccepted);
                Assert.IsNull(joinAccepter.AcceptedContainer);
            }
        }

        [UnityTest]
        public IEnumerator HostButton_Click_PortCorrect_PortInputGreen_UiChanged_EventsInvoked_SettingsSent()
        {
            string port = "1040";

            SetUp_HostInput(out GameObject hostUI, out GameObject loadingUI,
                out InputField portInput, out Image portInputImage,
                out SettingsTestDummy settingsAccepter, out SettingsTestDummy joinAccepter,
                out Button joinButton);

            yield return null;//skip frame

            portInput.text = port;

            yield return null;//skip frame

            joinButton.onClick.Invoke();

            yield return null;//skip frame

            Assert.AreEqual(Color.green, portInputImage.color);

            Assert.IsTrue(!hostUI.activeSelf);
            Assert.IsTrue(loadingUI.activeSelf);

            Assert.IsTrue(settingsAccepter.ContainerAccepted);

            var settingsContainer = settingsAccepter.AcceptedContainer;
            Assert.IsTrue(settingsContainer != null);
            Assert.AreEqual("TestUser", settingsContainer.ClientName);
            Assert.AreEqual("localhost", settingsContainer.Address);
            Assert.AreEqual(port, settingsContainer.Port.ToString());

            Assert.IsTrue(joinAccepter.ContainerAccepted);

            var joinContainer = joinAccepter.AcceptedContainer;
            Assert.IsTrue(joinContainer != null);
            Assert.AreEqual("TestUser", joinContainer.ClientName);
            Assert.AreEqual("localhost", joinContainer.Address);
            Assert.AreEqual(port, joinContainer.Port.ToString());

        }
    }
}
