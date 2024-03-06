using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;
using System.Linq;
using UnityEngine.XR.ARSubsystems;
using System.Net;

public class LMCCPinsController : MonoBehaviour
{

    static readonly string lmccDeviceIp = "127.0.0.1";
    readonly string lmccApiPinCall = "http://" + lmccDeviceIp + ":3001/api/v0?get=map_info";

    [SerializeField] GameObject worldPin;

    // serialized for debug
    [SerializeField] List<LMCCPin> lmccPins;
    [SerializeField] List<double> pinLatCoords;
    [SerializeField] List<double> pinLongCoords;
    [SerializeField] List<GameObject> worldLMCCPins;


    bool updatingPins;

    readonly double[] mapCenterLatLon = { 29.564882056524166, -95.081497230282139 };
    readonly double[] latLongToMeter = { 8.989e-6, 1.122e-5 };

    // Start is called before the first frame update
    void Start()
    {
        updatingPins = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!updatingPins) StartCoroutine(UpdateLMCCPins());
        UpdatePinsOnField();
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
                AddAndRemoveLMCCPins(webRequest);
                UpdatePinsOnField();
            }

        }
        yield return new WaitForSecondsRealtime(0.25f);
        updatingPins = false;
    }

    void UpdatePinsOnField()
    {
        foreach (LMCCPin pin in lmccPins)
        {
            if (!pin.worldPin)
            {
                double[] convertedCoords = ConvertLatLongToMeter(pin.coordinates);
                Vector3 worldPos = new Vector3((float)convertedCoords[0], 0, (float)convertedCoords[1]);
                pin.worldPin = Instantiate(worldPin, worldPos, Quaternion.identity);
            }
        }
    }

    double[] ConvertLatLongToMeter(double[] lmccPinCoords)
    {
        double latToX = (mapCenterLatLon[0] - lmccPinCoords[0]) * 111.111;
        double latToZ = (mapCenterLatLon[1] - lmccPinCoords[1]) * 111.111;
        return new double[] { latToX, latToZ };
    }

    void AddAndRemoveLMCCPins(UnityWebRequest webRequest)
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
                LMCCPin newPin = new LMCCPin { coordinates = new double[] { coords[0], coords[1] } };
                lmccPins.Add(newPin);
            }
        }

        for (int i = lmccPins.Count() - 1; i >= 0; i--)
        {
            bool containsCoords = featureLatCoords.Contains(lmccPins[i].coordinates[0]) &&
                featureLongCoords.Contains(lmccPins[i].coordinates[1]);
            if (!containsCoords)
            {
                if (lmccPins[i].worldPin) Destroy(lmccPins[i].worldPin);
                pinLatCoords.RemoveAt(i);
                pinLongCoords.RemoveAt(i);
                lmccPins.RemoveAt(i);
            }
        }
    }
}

[System.Serializable]
public class FeatureCollection
{
    public List<Feature> features;
}

[System.Serializable]
public class Feature
{
    public Geometry geometry;
}

[System.Serializable]
public class Geometry
{
    public double[] coordinates;
}

[System.Serializable]
public class LMCCPin
{
    public double[] coordinates;
    public GameObject worldPin = null;
}