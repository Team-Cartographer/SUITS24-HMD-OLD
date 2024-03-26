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

public class LMCCPinsController : MonoBehaviour
{

    static readonly string lmccDeviceIp = "127.0.0.1";
    readonly string lmccGetPinURL = "http://" + lmccDeviceIp + ":3001/api/v0?get=map_info";
    readonly string lmccPostPinURL = "http://" + lmccDeviceIp + ":3001/api/v0?map=add";

    [SerializeField] GameObject worldPin;

    // serialized for debug
    [SerializeField] List<LMCCPin> lmccPins;
    [SerializeField] List<double> pinLatCoords;
    [SerializeField] List<double> pinLongCoords;
    [SerializeField] List<GameObject> worldLMCCPins;


    bool updatingPins;

    readonly double[] mapCenterLatLon = { 29.564882056524166, -95.081497230282139 };
    readonly double[] latLongToMeter = { 110836.0, 97439.0 };

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

    IEnumerator UpdateLMCCPins() // attempts to get pins from api
    {
        updatingPins = true;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(lmccGetPinURL))
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

    public IEnumerator AddPinFromHMD(string pinJson)
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
    }

    void UpdatePinsOnField() // checks to see if any of the lmcc pins have a physical pin object in the world and creates one if not
    {
        foreach (LMCCPin pin in lmccPins)
        {
            if (!pin.worldPin)
            {
                double[] convertedCoords = ConvertLatLongToMeter(pin.coordinates);
                Vector3 worldPos = new Vector3((float)convertedCoords[0], 0, (float) convertedCoords[2]);
                pin.worldPin = Instantiate(worldPin, worldPos, Quaternion.identity);
            }
        }
    }

    public double[] ConvertLatLongToMeter(double[] lmccPinCoords) // approximate conversion; input format: [lat, long]
    {
        double latToZ = (mapCenterLatLon[0] - lmccPinCoords[0]) * 111111;
        double longToX = (mapCenterLatLon[1] - lmccPinCoords[1]) * 111111;
        return new double[] { latToZ, 0, longToX };
    }

    public double[] ConvertMeterToLatLong(Vector3 worldCoords) // input format
    {
        double xToLong = mapCenterLatLon[1] - worldCoords.x;
        double zToLat = mapCenterLatLon[0] - worldCoords.z;

        return new double[] {zToLat, xToLong};

        // m - x/c = l
    }

    void AddAndRemoveLMCCPins(UnityWebRequest webRequest)
    {
        // breaks down geojson
        FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(webRequest.downloadHandler.text);
        List<double> featureLatCoords = new List<double>();
        List<double> featureLongCoords = new List<double>();

        foreach (Feature feature in featureCollection.features)
        {
            // adds lmcc pins to list; checks to see if there's duplicate coords (i.e. pin already placed at coords);
            // if there are, it won't add an extra pin
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
    public bool isHMDPin = false;
}