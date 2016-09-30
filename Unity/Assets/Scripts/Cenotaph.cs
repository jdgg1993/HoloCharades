﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public string person;
    public string dateofdeath;
    public string placeofdeath;
    public string ageatdeath;
    public string Surname;
    public string Firstname;
    public string cemetery;
    public string gravereference;
    public string restingplace;
    public string locationA;
    public string locationB;
    public string locationC;
    public string locationD;
    public string locationE;
}
