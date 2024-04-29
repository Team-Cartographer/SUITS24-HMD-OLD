using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GatewayConnection : MonoBehaviour
{
    private string host;
    private string port;
    private string url;
    private bool connected;
    private float timeSinceLastUpdate = 0f;
    private float updateInterval = 1.5f; // 1 second

    private Dictionary<string, bool> isUpdated = new Dictionary<string, bool>
    {
        { "UIA", false }, { "DCU", false }, { "ROVER", false }, { "SPEC", false },
        { "TELEMETRY", false }, { "COMM", false }, { "IMU", false }, { "ERROR", false },
        { "ROCKDATA", false }, { "EVAINFO", false }, { "TSSINFO", false },
        { "TODO", false }, { "WARNING", false }, { "GEOJSON", false }
    };

    private Dictionary<string, string> jsonStrings = new Dictionary<string, string>
    {
        { "UIA", "" }, { "DCU", "" }, { "ROVER", "" }, { "SPEC", "" },
        { "TELEMETRY", "" }, { "COMM", "" }, { "IMU", "" }, { "ERROR", "" },
        { "ROCKDATA", "" }, { "EVAINFO", "" }, { "TSSINFO", "" },
        { "TODO", "" }, { "WARNING", "" }, { "GEOJSON", "" }
    };

    public void ConnectToHost(string host, int port)
    {
        this.host = host;
        this.port = port.ToString();
        this.url = $"http://{this.host}:{this.port}";
        Debug.Log(this.url);
        StartCoroutine(GetRequest(this.url));
    }

    public void DisconnectFromHost()
    {
        this.connected = false;
    }

    private void Update()
    {
        if (this.connected)
        {
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= updateInterval)
            {
                UpdateAllStates();
                timeSinceLastUpdate = 0f;
            }
        }
    }

    private void UpdateAllStates()
    {
        StartCoroutine(GetUIAState());
        StartCoroutine(GetDCUState());
        StartCoroutine(GetROVERState());
        StartCoroutine(GetSPECState());
        StartCoroutine(GetTELEMETRYState());
        StartCoroutine(GetCOMMState());
        StartCoroutine(GetIMUState());
        StartCoroutine(GetERRORState());
        StartCoroutine(GetROCKDATAState());
        StartCoroutine(GetEVAINFOState());
        StartCoroutine(GetTSSINFOState());
        StartCoroutine(GetTODOState());
        StartCoroutine(GetWARNINGState());
        StartCoroutine(GetGEOJSONState());
    }

    private IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"{pages[page]}: Error: {webRequest.error}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"{pages[page]}: HTTP Error: {webRequest.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log($"{pages[page]}:\nReceived: {webRequest.downloadHandler.text}");
                    this.connected = true;
                    break;
            }
        }
    }

    #region State Getters
    private IEnumerator GetUIAState() { yield return GetState("UIA", "/mission/uia"); }
    private IEnumerator GetDCUState() { yield return GetState("DCU", "/mission/dcu"); }
    private IEnumerator GetROVERState() { yield return GetState("ROVER", "/mission/rover"); }
    private IEnumerator GetSPECState() { yield return GetState("SPEC", "/mission/spec"); }
    private IEnumerator GetTELEMETRYState() { yield return GetState("TELEMETRY", "/tss/telemetry"); }
    private IEnumerator GetCOMMState() { yield return GetState("COMM", "/mission/comm"); }
    private IEnumerator GetIMUState() { yield return GetState("IMU", "/mission/imu"); }
    private IEnumerator GetERRORState() { yield return GetState("ERROR", "/mission/error"); }
    private IEnumerator GetROCKDATAState() { yield return GetState("ROCKDATA", "/tss/rockdata"); }
    private IEnumerator GetEVAINFOState() { yield return GetState("EVAINFO", "/tss/eva_info"); }
    private IEnumerator GetTSSINFOState() { yield return GetState("TSSINFO", "/tss/info"); }
    private IEnumerator GetTODOState() { yield return GetState("TODO", "/todo"); }
    private IEnumerator GetWARNINGState() { yield return GetState("WARNING", "/warning"); }
    private IEnumerator GetGEOJSONState() { yield return GetState("GEOJSON", "/geojson"); }
    #endregion

    private IEnumerator GetState(string stateName, string endpoint)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + endpoint))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.jsonStrings[stateName] != webRequest.downloadHandler.text)
                    {
                        this.isUpdated[stateName] = true;
                        this.jsonStrings[stateName] = webRequest.downloadHandler.text;
                    }
                    break;
            }
        }

        yield return new WaitForSeconds(updateInterval); 
    }

    public string GetJsonString(string stateName)
    {
        this.isUpdated[stateName] = false;
        return this.jsonStrings[stateName];
    }

    public bool IsStateUpdated(string stateName)
    {
        return this.isUpdated[stateName];
    }


    public string GetDCUJsonString()
    {
        return this.GetJsonString("DCU");
    }

    public string GetUIAJsonString()
    {
        return this.GetJsonString("UIA");
    }

    public string GetROVERJsonString()
    {
        return this.GetJsonString("ROVER");
    }

    public string GetSPECJsonString()
    {
        return this.GetJsonString("SPEC");
    }

    public string GetTELEMETRYJsonString()
    {
        return this.GetJsonString("TELEMETRY");
    }

    public string GetCOMMJsonString()
    {
        return this.GetJsonString("COMM");
    }

    public string GetIMUJsonString()
    {
        return this.GetJsonString("IMU");
    }

    public string GetERRORJsonString()
    {
        return this.GetJsonString("ERROR"); 
    }

    public string GetROCKDATAJsonString()
    {
        return this.GetJsonString("ROCKDATA");
    }

    public string GetEVAINFOJsonString()
    {
        return this.GetJsonString("EVAINFO");
    }

    public string GetTSSINFOJsonString()
    {
        return this.GetJsonString("TSSINFO");
    }

    public string GetTODOITEMSJsonString()
    {
        return this.GetJsonString("TODO"); 
    }

    public string GetWARNINGJsonString()
    {
        return this.GetJsonString("WARNING"); 
    }

    public string GetGEOJSONJsonString()
    {
        return this.GetJsonString("GEOJSON");
    }



    public bool isDCUUpdated()
    {
        return this.IsStateUpdated("DCU");
    }

    public bool isUIAUpdated()
    {
        return this.IsStateUpdated("UIA");
    }

    public bool isROVERUpdated()
    {
        return this.IsStateUpdated("ROVER");
    }

    public bool isSPECUpdated()
    {
        return this.IsStateUpdated("SPEC");
    }

    public bool isTELEMETRYUpdated()
    {
        return this.IsStateUpdated("TELEMETRY");
    }

    public bool isCOMMUpdated()
    {
        return this.IsStateUpdated("COMM");
    }

    public bool isIMUUpdated()
    {
        return this.IsStateUpdated("IMU");
    }

    public bool isERRORUpdated()
    {
        return this.IsStateUpdated("ERROR");
    }

    public bool isROCKDATAUpdated()
    {
        return this.IsStateUpdated("ROCKDATA");
    }

    public bool isEVAINFOUpdated()
    {
        return this.IsStateUpdated("EVAINFO");
    }

    public bool isTSSINFOUpdated()
    {
        return this.IsStateUpdated("TSSINFO");
    }

    public bool isTODOITEMSUpdated()
    {
        return this.IsStateUpdated("TODO");
    }

    public bool isWARNINGUpdated()
    {
        return this.IsStateUpdated("WARNING");
    }

    public bool isGEOJSONUpdated()
    {
        return this.IsStateUpdated("GEOJSON");
    }

}