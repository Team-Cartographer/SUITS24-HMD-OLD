using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Numerics;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using Newtonsoft.Json.Linq;

public class Breadcrumbs : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PlayerController playerController;
    public ConnectionHandler connectionHandler;
    private LineRenderer lineRenderer;
    

    private List<UnityEngine.Vector3> points = new List<UnityEngine.Vector3>();

    private double lastTime;
    private double interval = 3;

    void Start()
    {
        lastTime = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeAsDouble - lastTime > interval)
        {
            lastTime = Time.timeAsDouble;
            GatewayConnection conn = connectionHandler.GetConnection();

            if (conn == null || !conn.isIMUUpdated())
                return;

            string IMUstring = conn.GetIMUJsonString();

            JObject jo = JObject.Parse(IMUstring);

            float x1 = jo["imu"]["eva1"]["posx"].ToObject<float>();
            float y1 = jo["imu"]["eva1"]["posy"].ToObject<float>();

            float x2 = jo["imu"]["eva2"]["posx"].ToObject<float>();
            float y2 = jo["imu"]["eva2"]["posy"].ToObject<float>();

            float z = Camera.main.transform.position.z;

            playerController.PlacePin(x1, y1, z);
            playerController.PlacePin(x2, y2, z);
        }
    }
}
