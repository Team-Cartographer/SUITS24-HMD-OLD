using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Microsoft.MixedReality;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class Compass : MonoBehaviour
{
    public RawImage CompassImage;

    public float heading = 360;

    // Start is called before the first frame update
    public ConnectionHandler connectionHandler;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float cameraYaw = Camera.main.transform.rotation.y;
        float cameraOffset = 0;

        TSScConnection conn = connectionHandler.GetConnection();
        if (conn != null && conn.isIMUUpdated())
        {
            string IMUstring = conn.GetIMUJsonString();
            JObject jo = JObject.Parse(IMUstring);
            float tssHeading = jo["imu"]["eva1"]["heading"].ToObject<float>();

            cameraOffset = cameraOffset - tssHeading;
        }

        cameraYaw -= cameraOffset;

        CompassImage.uvRect = new Rect(cameraYaw / 360, 0, 1, 1);
    }
    public static string Strip(string S)
    {
        var arr = new char[S.Length];
        var cnt = 0;
        for (int i = 0; i < S.Length; i++)
        {
            switch (S[i])
            {
                case ' ':
                case '\r':
                case '\n':
                    break;

                default:
                    arr[cnt] = S[i];
                    cnt++;
                    break;
            }
        }

        return new string(arr, 0, cnt);
    }
}
