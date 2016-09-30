using UnityEngine;
using System.Collections;
using System;

public class Sketch : MonoBehaviour {

    public GameObject myPrefab;

    public string websiteURL = "http://infosys320azurelab.azurewebsites.net/tables/Cenotaph?ZUMO-API-VERSION=2.0.0";

    //public Material[] material;

    void Start()
    {

        WWW jsonResponse = GET(websiteURL);

        if (string.IsNullOrEmpty(jsonResponse.text))
        {
            return;
        }

        string text = "{\"items\":" + jsonResponse.text + "}";

        Cenotaphs cenotaphs = Cenotaphs.CreateFromJSON(text);

        Debug.Log("Number of records: " + cenotaphs.items.Length);

        int totalCubes = cenotaphs.items.Length;
        int totalDistance = 5;
        int i = 0;
        float cubeSize = 0f;
        int x1 = 0, x2 = 0, x3 = 0;
        foreach (CenotaphsItems cenotaph in cenotaphs.items)
        {
            float perc = i / (float)totalCubes;
            i++;
            float x = perc * totalDistance;
            float y = 0f;
            float z = 2.0f;

            int ageoAtDeath = int.Parse(cenotaph.ageatdeath);

            if (ageoAtDeath > 20 && ageoAtDeath <= 45)
            {
                y = 0.5f;
                x = (x1 / (float)totalCubes) * totalDistance + cubeSize;
                x1++;
            }
            else if (ageoAtDeath > 46 && ageoAtDeath <= 65)
            {
                y = 0f;
                x = (x2 / (float)totalCubes) * totalDistance + cubeSize;
                x2++;
            }
            else
            {
                y = -0.5f;
                x = (x3 / (float)totalCubes) * totalDistance + cubeSize;
                x3++;
            }

            GameObject newCube = (GameObject)Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
            cubeSize = 0.5f;
            newCube.GetComponent<myCubeScript>().setSize(cubeSize);
            newCube.GetComponent<myCubeScript>().ratateSpeed = perc;
            //newCube.GetComponentInChildren<TextMesh>().text = cenotaph.Surname;

            //if (cenotaph.placeofdeath.Contains("New Zealand"))
            //{
            //    newCube.GetComponent<Renderer>().material = material[0];
            //}
            //else if (cenotaph.placeofdeath.Contains("France"))
            //{
            //    newCube.GetComponent<Renderer>().material = material[1];
            //}
            //else if (cenotaph.placeofdeath.Contains("Scotland"))
            //{
            //    newCube.GetComponent<Renderer>().material = material[2];
            //}
            //else if (cenotaph.placeofdeath.Contains("Belgium"))
            //{
            //    newCube.GetComponent<Renderer>().material = material[3];
            //}
        }

        Debug.Log("Number of cubes created: " + i);
    }

    void Update()
    {

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
}
