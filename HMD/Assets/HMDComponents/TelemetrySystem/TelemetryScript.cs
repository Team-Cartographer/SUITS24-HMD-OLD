using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor.XR.LegacyInputHelpers;


public class TelemetryScript : MonoBehaviour
{
    public TMP_Text telemetryHeader;
    public TMP_Text telemetryBody;
    public Canvas telemetryCanvas;
    public TMP_Text activeTime;


    public ConnectionHandler connection;

    public int time;
    public JObject eva1;
    public JObject eva2;


    public static string FormatTime(int seconds)
    {
        int hours = seconds / 3600;
        int minutes = (seconds % 3600) / 60;
        int secs = seconds % 60;

        return $"{hours:D2}:{minutes:D2}:{secs:D2}";
    }

    public static string ConvertToString(JObject jObj)
    {
        if (jObj == null)
        {
            return "Null jObj was found";
        }
        string result = "";

        foreach (var pair in jObj)
        {
            result += $"{pair.Key} : {pair.Value}\n";
        }

        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        telemetryCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // Networking
        GatewayConnection conn = connection.GetConnection();

        if (conn != null && conn.isTELEMETRYUpdated())
        {
            Debug.Log(conn.GetTELEMETRYJsonString());
            JObject telemetryTotal = JObject.Parse(conn.GetTELEMETRYJsonString());
            JToken telemetry = telemetryTotal["telemetry"];
            time = telemetry["eva_time"].ToObject<int>();

            eva1 = telemetry["eva1"].ToObject<JObject>();
            eva2 = telemetry["eva2"].ToObject<JObject>();

            activeTime.text = FormatTime(time);
        }


        // displaying
        if (Input.GetKeyDown(KeyCode.X))  // Change to hand commands
        {
            telemetryCanvas.gameObject.SetActive(!telemetryCanvas.gameObject.activeSelf);
        }
        if (telemetryCanvas.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))  // Change to hand commands
            {
                telemetryHeader.text = "EVA 1 Telemetry Data:";

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))  // Change to hand commands
            {
                telemetryHeader.text = "EVA 2 Telemetry Data:";

            }

            if (telemetryHeader.text == "EVA 2 Telemetry Data:")
            {
                telemetryBody.text = ConvertToString(eva2);
            }
            else
            {
                telemetryBody.text = ConvertToString(eva1);
            }

        }
    }
}
