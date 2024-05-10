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
using System.Text;
using UnityEditor.PackageManager.Requests;
using Oculus.Interaction.PoseDetection;
using OVRSimpleJSON;

public class LMCCPinsController : MonoBehaviour
{
    [SerializeField] ConnectionHandler connectionHandler;
    GatewayConnection gatewayConnection;

    [SerializeField] GameObject worldPin;

    // serialized for debug
    [SerializeField] List<LMCCPin> lmccPins;
    [SerializeField] List<double> pinLatCoords;
    [SerializeField] List<double> pinLongCoords;
    [SerializeField] List<GameObject> worldLMCCPins;

    readonly int[] mapCenterUTM15 = { 1875, 1760 }; // x: 225 (left) - 3490 (right), y: 3290 (bottom) - 225 (top)
    readonly double[] mapCenterLatLon = { 29.564882056524166, -95.081497230282139 };
    readonly double[] latLongToMeter = { 110836.0, 97439.0 };

    // Start is called before the first frame update
    void Start()
    {
        gatewayConnection = connectionHandler.GetConnection();
    }

    // Update is called once per frame
    void Update()
    {
        if (gatewayConnection != null && gatewayConnection.isGEOJSONUpdated())
        {
            UpdateLMCCPins(gatewayConnection.GetGEOJSONJsonString());
            UpdatePinsOnField();
        }
    }

    /*public IEnumerator AddPinFromHMD(string pinJson)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(lmccPostPinURL, "POST"))
        {
            var inputAsBytes = Encoding.UTF8.GetBytes(pinJson);
            webRequest.uploadHandler = new UploadHandlerRaw(inputAsBytes);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            Debug.Log("Status Code: " + webRequest.responseCode);
        }
    }*/

    void UpdatePinsOnField() // checks to see if any of the lmcc pins have a physical pin object in the world and creates one if not
    {
        foreach (LMCCPin pin in lmccPins)
        {
            if (!pin.worldPin)
            {
                double[] convertedCoords = CenterUTMCoords(pin.coordinates);
                Vector3 worldPos = new Vector3((float) convertedCoords[0], 0, (float) convertedCoords[1]);
                pin.worldPin = Instantiate(worldPin, worldPos, Quaternion.identity);
            }
        }
    }

    public double[] CenterUTMCoords(double[] lmccPinCoords)
    {
        double xPos = lmccPinCoords[0] - (double) mapCenterUTM15[0];
        double zPos = mapCenterUTM15[1] - (double) lmccPinCoords[1];
        double[] convertedPosition = { xPos, zPos };
        return convertedPosition;
    }

    void UpdateLMCCPins(string jsonString)
    {
        JObject pinsJson = JObject.Parse(jsonString);
        JArray features = (JArray) pinsJson["features"];
        List<double> retrievedLatCoords = new List<double>();
        List<double> retrievedLongCoords = new List<double>();
        foreach (JObject feature in features)
        {
            JArray coordinates = (JArray)feature["geometry"]["coordinates"];
            double[] coords = { coordinates[0][0].Value<double>(), coordinates[0][1].Value<double>() };
            retrievedLatCoords.Add(coords[0]);
            retrievedLongCoords.Add(coords[1]);
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
            bool containsCoords = retrievedLatCoords.Contains(lmccPins[i].coordinates[0]) &&
                retrievedLongCoords.Contains(lmccPins[i].coordinates[1]);
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


// all these below are for each section of the json;
// most important is the class LMCCPin, which has coords, a physical pin, and a variable checking if it was placed on the HMD side first
[System.Serializable]
public class FeatureCollection
{
    public List<Feature> features;
}

[System.Serializable]
public class Feature
{
    public Point point;
}

[System.Serializable]
public class Point
{
    public int[][] coordinates;
}

[System.Serializable]
public class LMCCPin
{
    public double[] coordinates;
    public GameObject worldPin = null;
    public bool isHMDPin = false;
}


// Old code that may need to be used again in the future

/*void AddAndRemoveLMCCPins(string jsonString)
{
    // breaks down geojson
    FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(jsonString);
    List<double> featureLatCoords = new List<double>();
    List<double> featureLongCoords = new List<double>();

    Debug.Log("NUM FEATURES: " + featureCollection.features.Count);

    foreach (Feature feature in featureCollection.features)
    {
        // adds lmcc pins to list; checks to see if there's duplicate coords (i.e. pin already placed at coords);
        // if there are, it won't add an extra pin
        Debug.Log("Feature Point is null: " + (feature.point == null));
        Debug.Log("Coords exist: " + feature.point.coordinates[0][0].ToString() + " " + feature.point.coordinates[0][1].ToString());
        double[] coords = { 0,0 };
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

    // checks to see if any pins have been removed from the lmcc server, and will delete those that have been
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
}*/

/*public double[] ConvertLatLongToMeter(double[] lmccPinCoords) // approximate conversion; input format: [lat, long]
{
    double latToZ = (mapCenterLatLon[0] - lmccPinCoords[0]) * 111111;
    double longToX = (mapCenterLatLon[1] - lmccPinCoords[1]) * 111111;
    return new double[] { latToZ, 0, longToX };
}*/