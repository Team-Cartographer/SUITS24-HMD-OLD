using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class CriticalTelemetry : MonoBehaviour
{
    public TMP_Text bpmText;
    public TMP_Text battLife;
    public TMP_Text oxyTime;

    public ConnectionHandler connection;


    // Start is called before the first frame update
    void Start()
    {
        bpmText.gameObject.SetActive(true);
        battLife.gameObject.SetActive(true);
        oxyTime.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        /// CHECK/UPDATE TODOLIST 
        if (connection != null)
        {

            GatewayConnection conn = connection.GetConnection();

            string telem = conn.GetTELEMETRYJsonString();
            JObject jo = JObject.Parse(telem);

            bpmText.text = "BPM: ";
            
            float bpm1 = jo["telemetry"]["eva1"]["heart_rate"].ToObject<float>();
            bpmText.text += bpm1;

            if (jo["telemetry"]["eva1"]["heart_rate"].ToObject<float>() > 100) //change to off-nominal values
            {
                bpmText.color = Color.red;
            }
            else
            {
                bpmText.color = Color.green;
            }

            battLife.text = "Remaining Battery Life: ";
            float batlife = jo["telemetry"]["eva1"]["batt_time_left"].ToObject<float>();
            int bhours = (int)batlife / 3600;
            batlife %= 3600;
            int bminutes = (int)batlife / 60;
            batlife %= 60;

            battLife.text += $"{bhours}:{bminutes}:{(int)batlife}";

            oxyTime.text = "Remaining Oxygen Time: ";
            float oxytime = jo["telemetry"]["eva1"]["oxy_time_left"].ToObject<float>();

            int ohours = (int)oxytime / 3600;
            oxytime %= 3600;
            int ominutes = (int)oxytime / 60;
            oxytime %= 60;

            oxyTime.text += $"{ohours}:{ominutes}:{(int)oxytime}";

        }

    }

    void OpenWarning()
    {
        bpmText.gameObject.SetActive(true);
        battLife.gameObject.SetActive(true);
        //warningVignette.gameObject.SetActive(true);
    }

    void CloseWarning()
    {
        bpmText.gameObject.SetActive(false);
        battLife.gameObject.SetActive(false);
        //warningVignette.gameObject.SetActive(false);

    }
}
