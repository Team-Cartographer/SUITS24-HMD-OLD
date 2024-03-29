using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class GeoSampleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text geoText;
    public ConnectionHandler connectionHandler;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GatewayConnection conn = connectionHandler.GetConnection();
        if (conn != null && conn.isSPECUpdated())
        {
            string spec = conn.GetSPECJsonString();
            JObject jo = JObject.Parse(spec);

            float id1 = jo["spec"]["eva1"]["id"].ToObject<float>();
            float id2 = jo["spec"]["eva2"]["id"].ToObject<float>();
            string text = "";
            if (id1 != 0)
            {
                text += "Eva1\n";
                text += $"id: {id1}\n";


                foreach (var pair in jo["spec"]["eva1"]["data"].ToObject<JObject>())
                {
                    text += $"{pair.Key}: {pair.Value}\n";
                    //Debug.Log(pair.Key);
                }
                //geoText = $"eva1\n{}";
            }
            if (id2 != 0)
            {
                text += "Eva2\n";
                text += $"id: {id2}\n";


                foreach (var pair in jo["spec"]["eva2"]["data"].ToObject<JObject>())
                {
                    text += $"{pair.Key}: {pair.Value}\n";
                    //Debug.Log(pair.Key);
                }
                //geoText = $"eva1\n{}";
            }
            geoText.text = text;
        }
    }
}
