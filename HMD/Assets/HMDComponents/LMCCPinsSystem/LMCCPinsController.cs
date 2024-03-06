using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;
using System.Linq;
using UnityEngine.XR.ARSubsystems;


[System.Serializable]
public class FeatureCollection
{
    public List<Feature> features;
}

[System.Serializable]
public class Feature
{
    public Properties properties;
    public Geometry geometry;
}

[System.Serializable]
public class Properties
{
    public string name;
}
[System.Serializable]
public class Geometry
{
    public double[] coordinates;
}

[System.Serializable]
public class LMCCPin
{
    public string name;
    public double[] coordinates;
}

public class LMCCPinsController : MonoBehaviour
{

    static readonly string lmccDeviceIp = "127.0.0.1";
    readonly string lmccApiPinCall = "http://" + lmccDeviceIp + ":3001/api/v0?get=map_info";

    [SerializeField] List<LMCCPin> lmccPins;
    [SerializeField] List<double> pinLatCoords;
    [SerializeField] List<double> pinLongCoords;

    bool updatingPins;

    // Start is called before the first frame update
    void Start()
    {
        updatingPins = false;
        //if (!updatingPins) StartCoroutine(UpdateLMCCPins());
    }

    // Update is called once per frame
    void Update()
    {
        if (!updatingPins) StartCoroutine(UpdateLMCCPins());
    }

    IEnumerator UpdateLMCCPins()
    {
        updatingPins = true;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(lmccApiPinCall))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(webRequest.downloadHandler.text);
                List<double> featureLatCoords = new List<double>();
                List<double> featureLongCoords = new List<double>();

                foreach (Feature feature in featureCollection.features) 
                {
                    double[] coords = feature.geometry.coordinates;
                    featureLatCoords.Add(coords[0]);
                    featureLongCoords.Add(coords[1]);
                    if (!pinLatCoords.Contains(coords[0]) && !pinLongCoords.Contains(coords[1]))
                    {
                        pinLatCoords.Add(coords[0]);
                        pinLongCoords.Add(coords[1]);
                        LMCCPin newPin = new LMCCPin { name = feature.properties.name,
                            coordinates = coords };
                        lmccPins.Add(newPin);
                    }
                }

                for (int i = lmccPins.Count() - 1; i >= 0; i--)
                {
                    bool containsCoords = featureLatCoords.Contains(lmccPins[i].coordinates[0]) && 
                        featureLongCoords.Contains(lmccPins[i].coordinates[1]);
                    if (!containsCoords)
                    {
                        pinLatCoords.RemoveAt(i);
                        pinLongCoords.RemoveAt(i);
                        lmccPins.RemoveAt(i);
                    }
                }
            }

        }
        yield return new WaitForSecondsRealtime(0.25f);
        updatingPins = false;
    }
}
