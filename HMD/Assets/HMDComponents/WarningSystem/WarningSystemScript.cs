using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;
using Newtonsoft.Json;

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
                LMCCNotification lmccNotification = JsonConvert.DeserializeObject<LMCCNotification>(webRequest.downloadHandler.text);


                if (lmccNotification.isWarning)
                {
                    //warningText.text = "Warning:";
                    warningDetailsText.text = lmccNotification.infoWarning;
                    OpenWarning();
                }
                else CloseWarning();


                // look into optimizing this next code block
                bool allDone = true;
                if (lmccNotification.todoItems != null)
                {
                    foreach (var todoItem in lmccNotification.todoItems)
                    {
                        if (todoItem[1] != "True")
                        {
                            allDone = false;
                            break;
                        }
                    }
                }

                
                if (lmccNotification.todoItems == null || allDone)
                {  // This part is not optimized, but is designed to be readable
                    messageText.gameObject.SetActive(false);
                    messageDetailsText.gameObject.SetActive(false);
                }
                else
                {
                    messageText.gameObject.SetActive(true);
                    messageDetailsText.gameObject.SetActive(true);

                    //look into optimizing this next part
                    foreach (var todoItem in lmccNotification.todoItems)
                    {
                        if (todoItem[1] != "True")
                        {
                            messageDetailsText.text = todoItem[0];
                            break;
                        }
                    }
                }
                
            }
        }
        updatingWarnings = false;
    }
}
