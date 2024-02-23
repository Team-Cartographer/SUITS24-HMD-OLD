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
    public string[][] todoItems;
    public bool isWarning;
}

public class WarningSystemScript : MonoBehaviour
{
    public TMP_Text warningText;
    public TMP_Text warningDetailsText;
    public TMP_Text messageText;
    public TMP_Text messageDetailsText;
    public RawImage warningVignette;

    [SerializeField] static readonly string lmccDeviceIp = "127.0.0.1";
    static readonly string lmccApiCallGet = "http://" + lmccDeviceIp + ":3001/api/v0?get=notif";

    bool warningOccurring;
    bool updatingWarnings;

    // Start is called before the first frame update
    void Start()
    {
        warningOccurring = false;
        updatingWarnings = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
            if (!warningOccurring) OpenWarning();
            else CloseWarning();
        }
        if (!updatingWarnings) StartCoroutine(UpdateLMCCWarnings());
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

    IEnumerator UpdateLMCCWarnings()
    {
        updatingWarnings = true;
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
                LMCCNotification lmccNotification = JsonUtility.FromJson<LMCCNotification>(webRequest.downloadHandler.text);


                if (lmccNotification.isWarning)
                {
                    //warningText.text = "Warning:";
                    warningDetailsText.text = lmccNotification.infoWarning;
                    OpenWarning();
                }
                else CloseWarning();
                
                string[][] todoItems = lmccNotification.todoItems;
                Debug.Log("todo[0][0]");
                Debug.Log(todoItems[0][0]);

                messageDetailsText.text = todoItems[0][0];
            }
        }
        updatingWarnings = false;
    }
}
