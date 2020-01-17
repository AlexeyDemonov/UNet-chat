using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

public class SettingsManager
{
    readonly string _filename = "Settings.xml";

    SettingsContainer DefaultSettings => new SettingsContainer() { ClientName = "user", Address = "localhost", Port = 7777 };
    SettingsContainer LastLoadedSettings;

    public SettingsContainer Load()
    {
        if(!File.Exists(_filename))
            return DefaultSettings;

        try
        {
            using (var stream = File.OpenRead(_filename))
            {
                var serializer = new XmlSerializer(typeof(SettingsContainer));
                var loadedSettings = (SettingsContainer)serializer.Deserialize(stream);
                LastLoadedSettings = loadedSettings;
                return loadedSettings;
            }
        }
        catch (Exception)
        {
            return DefaultSettings;
        }
    }

    public void Save(SettingsContainer settings)
    {
        if(ValidateSettingsAreNew(settings))
        {
            try
            {
                using (var stream = File.Create(_filename))
                {
                    var serializer = new XmlSerializer(typeof(SettingsContainer));
                    serializer.Serialize(stream, settings);
                }
            }
            catch (Exception)
            {
                /*Do nothing*/
            }
        }
    }

    public async Task SaveAsync(SettingsContainer container)
    {
        await Task.Run(() => Save(container));
    }

    bool ValidateSettingsAreNew(SettingsContainer settings)
    {
        if(LastLoadedSettings == null)
            return true;
        else
        {
            return settings.ClientName != LastLoadedSettings.ClientName
                ||
                settings.Address != LastLoadedSettings.Address
                ||
                settings.Port != LastLoadedSettings.Port;
        }
    }
}