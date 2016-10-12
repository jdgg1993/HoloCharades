using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;

public class Sketch : MonoBehaviour {

    public GameObject myPrefab;
    public TextMesh description;

    public string websiteURL = "http://infosys320groupproject.azurewebsites.net/tables/specimen?ZUMO-API-VERSION=2.0.0";

    private GameObject newCube;
    private Cenotaphs cenotaphs;
    private Facts facts;
    private bool update = false;
    private bool changeFacts = false;
    private int i = -1;
    private int k = 0;
    private TimeSpan timeInAir;
    private List<FactItems> fi = new List<FactItems>();

    public ChatMessageViewModel ChatVM { get; set; } = new ChatMessageViewModel();
    public HubConnection conn { get; set; }
    public IHubProxy proxy { get; set; }

    void Start()
    {

        conn = new HubConnection("http://socketserverrelay.azurewebsites.net");
        conn.Error += (e) =>
        {
            //loadingText.text = "Error establishing connection";
        };

        proxy = conn.CreateHubProxy("ChatHub");
        proxy.On<ChatMessage>("broadcastMessage", (ChatMessage msg) =>
        {
            if (!msg.Username.Equals("HoloLens"))
            {
                ChatVM.Messages.Add(msg);
                Debug.Log(msg.Message);
                i++;
                update = true;
            }
        });

        conn.Start();

        WWW jsonResponse = GET(websiteURL);

        if (string.IsNullOrEmpty(jsonResponse.text))
            return;

        string text = "{\"items\":" + jsonResponse.text + "}";

        cenotaphs = Cenotaphs.CreateFromJSON(text);

        WWW jsonResponseFacts = GET("http://infosys320groupproject.azurewebsites.net/tables/facts?ZUMO-API-VERSION=2.0.0");

        if (string.IsNullOrEmpty(jsonResponseFacts.text))
            return;

        string textFacts = "{\"items\":" + jsonResponseFacts.text + "}";

        facts = Facts.CreateFromJSON(textFacts);

        int totalCubes = cenotaphs.items.Length;
        float cubeSize = 0f;
        Randomizer.Randomize(cenotaphs.items);

        CenotaphsItems cenotaph = cenotaphs.items[0];

        newCube = (GameObject)Instantiate(myPrefab, new Vector3(0, 0, 2), Quaternion.identity);
        cubeSize = 0.5f;
        newCube.transform.Rotate(new Vector3(0, 0, 180));
        newCube.GetComponent<myCubeScript>().setSize(cubeSize);
        newCube.GetComponent<myCubeScript>().ratateSpeed = 0;

        description = (TextMesh)Instantiate(description);
        description.text = "Ready";
        description.transform.position = new Vector3(0, newCube.transform.lossyScale.y, 2);
    }

    void Update()
    {
        timeInAir += TimeSpan.FromSeconds(Time.deltaTime);
        if ((timeInAir.Seconds == 30 || timeInAir.Seconds == 1) && fi.Count > 0 && !changeFacts)
        {
            k++;
            if (k < fi.Count)
                description.text = fi[k].fact;
            else
                k = 0;

            changeFacts = true;
        }
        if (timeInAir.Seconds > 30 || (timeInAir.Seconds < 30 && timeInAir.Seconds > 1))
            changeFacts = false;

        if (update && i < cenotaphs.items.Length)
        {
            fi.Clear();
            k = 0;
            for (int j = 0; j < facts.items.Length; j++)
            {
                if (facts.items[j].specimenId == cenotaphs.items[i].id)
                    fi.Add(facts.items[j]);
            }
            newCube.GetComponent<Renderer>().material.mainTexture = GET(cenotaphs.items[i].image).texture;
            description.text = fi[k].fact;

            proxy.Invoke("Send", new ChatMessage { Username = "HoloLens", Message = cenotaphs.items[i].commonName });
            update = false;
        }
        else if (update && i >= cenotaphs.items.Length)
        {
            proxy.Invoke("Send", new ChatMessage { Username = "HoloLens", Message = "Done" });
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
