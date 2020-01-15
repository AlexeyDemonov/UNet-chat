using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

public class SettingsManager
{
    readonly string _filename = "Settings.xml";

    SettingsContainer DefaultSettings => new SettingsContainer() { ClientName = "user", Address = "localhost", Port = 7777 };

    public SettingsContainer Load()
    {
        if(!File.Exists(_filename))
            return DefaultSettings;

        try
        {
            using (var stream = File.OpenRead(_filename))
            {
                var serializer = new XmlSerializer(typeof(SettingsContainer));
                return (SettingsContainer)serializer.Deserialize(stream);
            }
        }
        catch (Exception)
        {
            return DefaultSettings;
        }
    }

    public void Save(SettingsContainer settings)
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

    public async Task SaveAsync(SettingsContainer container)
    {
        await Task.Run(() => Save(container));
    }
}