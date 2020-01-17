using System;
using UnityEngine;

public class LogonInitializer : MonoBehaviour
{
    public LogonUIManager LogonUIManager;
    SettingsManager SettingsManager;
    ConnectionSetupManager ConnectionSetupManager;

    private void Awake()
    {
        SettingsManager = new SettingsManager();
        ConnectionSetupManager = GameObject.FindObjectOfType<ConnectionSetupManager>();

        LogonUIManager.Request_LoadSettings += SettingsManager.Load;
        LogonUIManager.Request_SaveSettings += SaveSettings;
        LogonUIManager.Request_Join += ConnectionSetupManager.Join;
        LogonUIManager.Request_Host += ConnectionSetupManager.Host;
    }

    void SaveSettings(SettingsContainer settings)
    {
        /*suppress warning*/_ = /**/SettingsManager.SaveAsync(settings);
    }

    private void OnDestroy()
    {
        LogonUIManager.Request_LoadSettings -= SettingsManager.Load;
        LogonUIManager.Request_SaveSettings -= SaveSettings;
        LogonUIManager.Request_Join -= ConnectionSetupManager.Join;
        LogonUIManager.Request_Host -= ConnectionSetupManager.Host;

        SettingsManager = null;
        ConnectionSetupManager = null;
    }
}