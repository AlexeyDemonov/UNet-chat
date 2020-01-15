using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Settings")]
public class SettingsContainer
{
    [XmlElement]
    public string ClientName { get; set; }
    [XmlElement]
    public string Address { get; set; }
    [XmlElement]
    public int Port { get; set; }
}