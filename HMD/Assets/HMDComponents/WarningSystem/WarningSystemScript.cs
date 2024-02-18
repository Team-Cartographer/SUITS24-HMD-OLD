using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;

[System.Serializable]
public class LMCCNotification
{
    public string infoWarning;
    public string infoTodo;
    public bool isWarning;
}

public class WarningSystemScript : MonoBehaviour
{
    public TMP_Text warningText;
    public TMP_Text warningDetailsText;
    public RawImage warningVignette;

    [SerializeField] static readonly string lmccDeviceIp = "169.234.98.214";
    static readonly string lmccApiCallGet = "http://" + lmccDeviceIp + "/api/v0?get=notif";

    bool warningOccurring;
    LMCCNotification lmccNotification;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
            if (!warningOccurring) OpenWarning();
            else CloseWarning();
        }
        UpdateLMCCWarnings();
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

    void UpdateLMCCWarnings()
    {
        StartCoroutine(GetLMCCWarningRequest());
        if (lmccNotification.isWarning)
        {
            //warningText.text = "Warning:";
            warningDetailsText.text = lmccNotification.infoWarning;
            OpenWarning();
        }
        else CloseWarning();
    }

    IEnumerator GetLMCCWarningRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(lmccApiCallGet))
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
                lmccNotification = JsonUtility.FromJson<LMCCNotification>(webRequest.downloadHandler.text);
            }
        }
    }
}
