﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
//using Microsoft.AspNet.SignalR.Client;

public class Sketch : MonoBehaviour {

    public GameObject myPrefab;

    public string websiteURL = "http://infosys320azurelab.azurewebsites.net/tables/Cenotaph?ZUMO-API-VERSION=2.0.0";

    private GameObject newCube;
    private bool update = false;
    private Cenotaphs cenotaphs;
    private int i = 0;

    //public ChatMessageViewModel ChatVM { get; set; } = new ChatMessageViewModel();
    //public HubConnection conn { get; set; }
    //public IHubProxy proxy { get; set; }

    void Start()
    {

        //conn = new HubConnection("http://socketserverrelay.azurewebsites.net");
        //conn.Error += (e) =>
        //{
        ////loadingText.text = "Error establishing connection";
        //};

        //proxy = conn.CreateHubProxy("ChatHub");
        //proxy.On<ChatMessage>("broadcastMessage", (ChatMessage msg) =>
        //{
        //    ChatVM.Messages.Add(msg);
        //    Debug.Log(msg.Message);
        //    i++;
        //    update = true;
        //});

        //conn.Start();

        WWW jsonResponse = GET(websiteURL);

        if (string.IsNullOrEmpty(jsonResponse.text))
            return;

        string text = "{\"items\":" + jsonResponse.text + "}";

        cenotaphs = Cenotaphs.CreateFromJSON(text);

        Debug.Log("Number of records: " + cenotaphs.items.Length);

        int totalCubes = cenotaphs.items.Length;
        float cubeSize = 0f;
        Randomizer.Randomize(cenotaphs.items);

        CenotaphsItems cenotaph = cenotaphs.items[0];

        newCube = (GameObject)Instantiate(myPrefab, new Vector3(0, 0, 2), Quaternion.identity);
        cubeSize = 0.5f;
        newCube.GetComponent<myCubeScript>().setSize(cubeSize);
        newCube.GetComponent<myCubeScript>().ratateSpeed = 1;
        newCube.GetComponentInChildren<TextMesh>().text = cenotaph.Surname;
    }

    void Update()
    {
        if (update && i < cenotaphs.items.Length)
        {
            newCube.GetComponentInChildren<TextMesh>().text = cenotaphs.items[i].Surname;
            update = false;
        }
    }

    public WWW GET(string url)
    {
        WWW www = new WWW(url);

        StartCoroutine(WaitForRequest(www));

        while (!www.isDone) { }

        return www;
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
    }

    public class Randomizer
    {
        public static void Randomize<T>(T[] items)
        {
            System.Random rand = new System.Random();

            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }
    }
}
