using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;


[System.Serializable]
public class FeatureCollection
{
    public List<Feature> features;
}

[System.Serializable]
public class Feature
{
    public Properties properties;
}

[System.Serializable]
public class Properties
{
    public string name;
}

[System.Serializable]
public class LMCCPin
{
    public string name;
}

public class LMCCPinsController : MonoBehaviour
{

    static readonly string lmccDeviceIp = "127.0.0.1";
    readonly string lmccApiPinCall = "http://" + lmccDeviceIp + ":3001/api/v0?get=map_info";

    [SerializeField] List<LMCCPin> lmccPins;

    bool updatingPins;

    // Start is called before the first frame update
    void Start()
    {
        updatingPins = false;
        if (!updatingPins) StartCoroutine(UpdateLMCCPins());
    }

    // Update is called once per frame
    void Update()
    {
        //if (!updatingPins) StartCoroutine(UpdateLMCCPins());
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
                foreach (Feature feature in featureCollection.features) 
                {
                    LMCCPin newPin = new LMCCPin { name = feature.properties.name };
                    Debug.Log(newPin.name);

                }
            }

        }
        updatingPins = false;
    }
}
