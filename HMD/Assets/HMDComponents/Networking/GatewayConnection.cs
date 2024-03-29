using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GatewayConnection : MonoBehaviour
{
    // Connection
    string host;
    string port;
    string url;
    bool connected;
    float time_since_last_update;

    // Database Jsons
    bool UIAUpdated;
    string UIAJsonString;
    bool DCUUpdated;
    string DCUJsonString;
    bool ROVERUpdated;
    string ROVERJsonString;
    bool SPECUpdated;
    string SPECJsonString;
    bool TELEMETRYUpdated;
    string TELEMETRYJsonString;
    bool COMMUpdated;
    string COMMJsonString;
    bool IMUUpdated;
    string IMUJsonString;
    bool ERRORUpdated;
    string ERRORJsonString;
    bool ROCKDATAUpdated;
    string ROCKDATAJsonString;
    bool EVAINFOUpdated;
    string EVAINFOJsonString;
    bool TSSINFOUpdated;
    string TSSINFOJsonString;

    ////// API ROUTES
    bool TODOUpdated;
    string TODOITEMSJsonString;
    bool WARNINGUpdated;
    string WARNINGJsonString;


    // Connect with port
    public void ConnectToHost(string host, int port)
    {
        this.host = host;
        this.port = port.ToString();
        this.url = "http://" + this.host + ":" + this.port;
        Debug.Log(this.url);

        // Test Connection
        StartCoroutine(GetRequest(this.url));
    }

    public void DisconnectFromHost()
    {
        this.connected = false;
    }

    // This Function is called when the program begins
    void Start()
    {
        time_since_last_update = 0.0f; 
    }

    // This Function is called each render frame
    void Update()
    {
        // If you are connected to TSSc
        if (this.connected)
        {
            // Each Second
            time_since_last_update += Time.deltaTime;
            if (time_since_last_update >= 1.0f)
            {
                // Pull TSSc Updates
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

                time_since_last_update = 0.0f;
            }
        }
    }

    IEnumerator GetRequest(string uri)
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
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    this.connected = true;
                    break;
            }

        }
    }

    ///////////////////////////////////////////// UIA

    IEnumerator GetUIAState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/mission/uia"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.UIAJsonString != webRequest.downloadHandler.text)
                    {
                        this.UIAUpdated = true;
                        this.UIAJsonString = webRequest.downloadHandler.text;
                    }
                    break;
            }

        }
    }

    public string GetUIAJsonString()
    {
        UIAUpdated = false;
        return this.UIAJsonString;
    }

    public bool isUIAUpdated()
    {
        return UIAUpdated;
    }

    ///////////////////////////////////////////// DCU

    IEnumerator GetDCUState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/mission/dcu"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.DCUJsonString != webRequest.downloadHandler.text)
                    {
                        this.DCUUpdated = true;
                        this.DCUJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.DCUJsonString);
                    }
                    break;
            }

        }
    }

    public string GetDCUJsonString()
    {
        DCUUpdated = false;
        return this.DCUJsonString;
    }

    public bool isDCUUpdated()
    {
        return DCUUpdated;
    }

    ///////////////////////////////////////////// ROVER

    IEnumerator GetROVERState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/mission/rover"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.ROVERJsonString != webRequest.downloadHandler.text)
                    {
                        this.ROVERUpdated = true;
                        this.ROVERJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.ROVERJsonString);
                    }
                    break;
            }

        }
    }

    public string GetROVERJsonString()
    {
        ROVERUpdated = false;
        return this.ROVERJsonString;
    }

    public bool isROVERUpdated()
    {
        return ROVERUpdated;
    }

    ///////////////////////////////////////////// SPEC

    IEnumerator GetSPECState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/mission/spec"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.SPECJsonString != webRequest.downloadHandler.text)
                    {
                        this.SPECUpdated = true;
                        this.SPECJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.SPECJsonString);
                    }
                    break;
            }

        }
    }

    public string GetSPECJsonString()
    {
        SPECUpdated = false;
        return this.SPECJsonString;
    }

    public bool isSPECUpdated()
    {
        return SPECUpdated;
    }


    ///////////////////////////////////////////// ERROR
    IEnumerator GetERRORState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/mission/error"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.ERRORJsonString != webRequest.downloadHandler.text)
                    {
                        this.ERRORUpdated = true;
                        this.ERRORJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.ERRORJsonString);
                    }
                    break;
            }

        }
    }

    public string GetERRORJsonString()
    {
        ERRORUpdated = false;
        return this.ERRORJsonString;
    }

    public bool isERRORUpdated()
    {
        return ERRORUpdated;
    }


    ///////////////////////////////////////////// TELEMETRY

    IEnumerator GetTELEMETRYState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/tss/telemetry"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.TELEMETRYJsonString != webRequest.downloadHandler.text)
                    {
                        this.TELEMETRYUpdated = true;
                        this.TELEMETRYJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.TELEMETRYJsonString);
                    }
                    break;
            }

        }
    }

    public string GetTELEMETRYJsonString()
    {
        TELEMETRYUpdated = false;
        return this.TELEMETRYJsonString;
    }

    public bool isTELEMETRYUpdated()
    {
        return TELEMETRYUpdated;
    }

    ///////////////////////////////////////////// COMM

    IEnumerator GetCOMMState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/mission/comm"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.COMMJsonString != webRequest.downloadHandler.text)
                    {
                        this.COMMUpdated = true;
                        this.COMMJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.COMMJsonString);
                    }
                    break;
            }

        }
    }

    public string GetCOMMJsonString()
    {
        COMMUpdated = false;
        return this.COMMJsonString;
    }

    public bool isCOMMUpdated()
    {
        return COMMUpdated;
    }

    ///////////////////////////////////////////// IMU

    IEnumerator GetIMUState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/mission/imu"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.IMUJsonString != webRequest.downloadHandler.text)
                    {
                        this.IMUUpdated = true;
                        this.IMUJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.IMUJsonString);
                    }
                    break;
            }

        }
    }

    public string GetIMUJsonString()
    {
        IMUUpdated = false;
        return this.IMUJsonString;
    }

    public bool isIMUUpdated()
    {
        return IMUUpdated;
    }

    ///////////////////////////////////////////// ROCKDATA

    IEnumerator GetROCKDATAState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/tss/rockdata"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.ROCKDATAJsonString != webRequest.downloadHandler.text)
                    {
                        this.ROCKDATAUpdated = true;
                        this.ROCKDATAJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.ROCKDATAJsonString);
                    }
                    break;
            }

        }
    }

    public string GetROCKDATAJsonString()
    {
        ROCKDATAUpdated = false;
        return this.ROCKDATAJsonString;
    }

    public bool isROCKDATAUpdated()
    {
        return ROCKDATAUpdated;
    }

    ///////////////////////////////////////////// EVAINFO

    IEnumerator GetEVAINFOState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/tss/eva_info"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.EVAINFOJsonString != webRequest.downloadHandler.text)
                    {
                        this.EVAINFOUpdated = true;
                        this.EVAINFOJsonString = webRequest.downloadHandler.text;
                        //Debug.Log(this.EVAINFOJsonString);
                    }
                    break;
            }

        }
    }

    public string GetEVAINFOJsonString()
    {
        EVAINFOUpdated = false;
        return this.EVAINFOJsonString;
    }

    public bool isEVAINFOUpdated()
    {
        return EVAINFOUpdated;
    }

    ///////////////////////////////////////////// TSSINFO

    IEnumerator GetTSSINFOState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/tss/info"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.TSSINFOJsonString != webRequest.downloadHandler.text)
                    {
                        this.TSSINFOUpdated = true;
                        this.TSSINFOJsonString = webRequest.downloadHandler.text;
                        Debug.Log(this.TSSINFOJsonString);
                    }
                    break;
            }

        }
    }

    public string GetTSSINFOJsonString()
    {
        TSSINFOUpdated = false;
        return this.TSSINFOJsonString;
    }

    public bool isTSSINFOUpdated()
    {
        return TSSINFOUpdated;
    }

    ///////////////////////////////////////////// API ROUTES

    IEnumerator GetTODOState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/api/v0?get=todo"))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.TODOITEMSJsonString != webRequest.downloadHandler.text)
                    {
                        this.TODOUpdated = true;
                        this.TODOITEMSJsonString = webRequest.downloadHandler.text;
                    }
                    break;
            }
        }
    }

    public string GetTODOITEMSJsonString()
    {
        TODOUpdated = false;
        return this.TODOITEMSJsonString;
    }

    public bool isTODOITEMSUpdated()
    {
        return TODOUpdated;
    }


    IEnumerator GetWARNINGState()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(this.url + "/api/v0?get=warning"))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if (this.WARNINGJsonString != webRequest.downloadHandler.text)
                    {
                        this.WARNINGUpdated = true;
                        this.WARNINGJsonString = webRequest.downloadHandler.text;
                    }
                    break;
            }
        }
    }

    public string GetWARNINGJsonString()
    {
        WARNINGUpdated = false;
        return this.WARNINGJsonString;
    }

    public bool isWARNINGUpdated()
    {
        return WARNINGUpdated;
    }
}