using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LogonUIManager : MonoBehaviour
{
    [Header("Error message")]
    public GameObject ErrorUI;
    public Text ErrorMessage;
    public Button ErrorAcceptButton;
    [Header("Name input")]
    public GameObject NameInputUI;
    public InputField NameInput;
    public Button NameConfirmButton;
    [Header("Mode choice")]
    public GameObject ModeChoiceUI;
    public Button JoinModeButton;
    public Button HostModeButton;
    [Header("Join mode")]
    public GameObject JoinModeUI;
    public InputField JoinAddressInput;
    public InputField JoinPortInput;
    public Button JoinButton;
    [Header("Host mode")]
    public GameObject HostModeUI;
    public InputField HostPortInput;
    public Button HostButton;
    [Header("Loading")]
    public GameObject LoadingUI;

    public event Func<SettingsContainer> Request_LoadSettings;
    public event Action<SettingsContainer> Request_SaveSettings;
    public event Action<SettingsContainer> Request_Join;
    public event Action<SettingsContainer> Request_Host;

    // Start is called before the first frame update
    void Start()
    {
        //Load settings if there any
        var container = Request_LoadSettings?.Invoke();
        if(container != null)
        {
            NameInput.text = container.ClientName;
            JoinAddressInput.text = container.Address;
            JoinPortInput.text = container.Port.ToString();
            HostPortInput.text = container.Port.ToString();
        }

        //Add listeners to all the buttons
        ErrorAcceptButton?.onClick.AddListener( () =>
        {
            ErrorUI.SetActive(false);
            NameInputUI.SetActive(true);
        });

        NameConfirmButton?.onClick.AddListener( () =>
        {
            bool nameValid = ValidateField(NameInput, (userInput) => ValidateName(userInput));

            if(nameValid)
            {
                NameInputUI.SetActive(false);
                ModeChoiceUI.SetActive(true);
            }
        });

        JoinModeButton?.onClick.AddListener( () =>
        {
            ModeChoiceUI.SetActive(false);
            JoinModeUI.SetActive(true);
        });

        HostModeButton?.onClick.AddListener(() =>
        {
            ModeChoiceUI.SetActive(false);
            HostModeUI.SetActive(true);
        });

        JoinButton?.onClick.AddListener( () =>
        {
            bool addressValid = ValidateField(JoinAddressInput, (userInput) => ValidateAddress(userInput));
            bool portValid = ValidateField(JoinPortInput, (userInput) => ValidatePort(userInput));

            if(addressValid && portValid)
            {
                JoinModeUI.SetActive(false);
                LoadingUI.SetActive(true);

                var newContainer = new SettingsContainer() { ClientName = NameInput.text, Address = JoinAddressInput.text, Port = int.Parse(JoinPortInput.text) };

                Request_SaveSettings?.Invoke(newContainer);
                Request_Join?.Invoke(newContainer);
            }
        });

        HostButton?.onClick.AddListener( () =>
        {
            bool portValid = ValidateField(HostPortInput, (userInput) => ValidatePort(userInput));

            if(portValid)
            {
                HostModeUI.SetActive(false);
                LoadingUI.SetActive(true);

                var newContainer = new SettingsContainer() { ClientName = NameInput.text, Address = "localhost", Port = int.Parse(HostPortInput.text)};

                Request_SaveSettings?.Invoke(newContainer);
                Request_Host?.Invoke(newContainer);
            }
        });

        //Show UI
        if (PropertyBag.ErrorMessage != null)
        {
            ErrorMessage.text = PropertyBag.ErrorMessage;
            PropertyBag.ErrorMessage = null;
            ErrorUI.SetActive(true);
        }
        else
            NameInputUI?.SetActive(true);
    }

    bool ValidateField(InputField inputField, Func<string, bool> validator)
    {
        bool valid = validator.Invoke(inputField.text);

        if (valid)
            inputField.GetComponent<Image>().color = Color.green;
        else
            inputField.GetComponent<Image>().color = Color.red;

        return valid;
    }

    bool ValidateName(string userInput)
    {
        return (userInput.ToLower() != "server" && userInput.Length > 2 && userInput.Length < 17);
    }

    bool ValidateAddress(string userInput)
    {
        if(userInput == "localhost")
            return true;

        if(!Regex.IsMatch(userInput, "^[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}$"))
            return false;

        var numbers = userInput.Split('.');

        foreach (var number in numbers)
        {
            if(!byte.TryParse(number, out byte _))//Just checking that it is a 0-255 number
                return false;
        }

        return true;
    }

    bool ValidatePort(string userInput)
    {
        if (int.TryParse(userInput, out int port))
            return port > 1023 && port < 49152;
        else
            return false;
    }
}
