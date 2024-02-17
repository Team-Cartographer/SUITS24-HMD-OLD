using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class SteamGame // for testing, will delete
{
    public string name;
    public string developer;
}

[System.Serializable]
public class LMCCNotification // for testing, will delete
{
    public string infoWarning;
    public string infoTodo;
    public string isWarning;
}

public class WarningSystemScript : MonoBehaviour
{
    public TMP_Text warningText;
    public TMP_Text warningDetailsText;
    public RawImage warningVignette;

    [SerializeField] static readonly string lmccDeviceIp = "127.0.0.1";
    static readonly string lmccApiCall = "http://" + lmccDeviceIp + "/api/v0?get=notif";

    bool warningOccurring;


    /*public static async String[] ApiCaller
    {
        Debug.Log("Hello world");
    }*/

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("steamspy.com/api.php?request=appdetails&appid=730"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
            if (!warningOccurring) OpenWarning();
            else CloseWarning();
        }


    }

    void OpenWarning(){
        warningOccurring = true;
        warningText.gameObject.SetActive(true);
        warningDetailsText.gameObject.SetActive(true);
        warningVignette.gameObject.SetActive(true);
    }

    void CloseWarning(){
        warningText.gameObject.SetActive(false);
        warningDetailsText.gameObject.SetActive(false);
        warningVignette.gameObject.SetActive(false);
        warningOccurring = false;
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Send request and wait for response
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Print response to console
                //Debug.Log("API Response: " + webRequest.downloadHandler.text);
                SteamGame csgo = JsonUtility.FromJson<SteamGame>(webRequest.downloadHandler.text);
                Debug.Log(csgo.name);
            }
        }
    }

}
