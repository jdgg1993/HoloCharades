using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[System.Serializable]
public class Cenotaphs
{
    public CenotaphsItems[] items;

    public static Cenotaphs CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Cenotaphs>(jsonString);
    }
}

[System.Serializable]
public class CenotaphsItems
{
    public string id;
    public string createdAt;
    public string updatedAt;
    public string version;
    public bool deleted;
    public object identifier;
    public string commonName;
    public string maoriName;
    public string image;
}

public class ChatMessage
{
    public string Username { get; set; }
    public string Message { get; set; }
}

public class ChatMessageViewModel
{
    public ObservableCollection<ChatMessage> Messages { get; set; } = new ObservableCollection<ChatMessage>();
}
