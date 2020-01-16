using UnityEngine;

public static class PropertyBag
{
    public static string ErrorMessage { get; set; }
    public static string ClientName { get; set; }

    public static GlobalChatManager GlobalChatManager {get; set;}
    public static ChatGameManager ChatGameManager { get; set;}

    public static ChatUIManager LocalChatUIManager;
}