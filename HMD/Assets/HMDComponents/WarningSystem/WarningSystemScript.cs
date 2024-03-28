using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;
using Newtonsoft.Json;


[System.Serializable]
public class LMCCWarning
{
    public string infoWarning;
}

[System.Serializable]
public class LMCCTodoItems
{
    public string[][] todoItems;
}

public class WarningSystemScript : MonoBehaviour
{
    public TMP_Text warningText;
    public TMP_Text warningDetailsText;
    public TMP_Text messageText;
    public TMP_Text messageDetailsText;
    public RawImage warningVignette;

    [SerializeField] static readonly string lmccDeviceIp = "192.168.4.36";
    static readonly string lmccApiCallGetWarning = "http://" + lmccDeviceIp + ":3001/api/v0?get=warning";
    static readonly string lmccApiCallGetTodo = "http://" + lmccDeviceIp + ":3001/api/v0?get=todo";

    float timeSinceLastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastUpdate = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        timeSinceLastUpdate += Time.deltaTime;
        //Debug.Log(timeSinceLastUpdate);
        if (timeSinceLastUpdate >= 0.5f)
        {
            StartCoroutine(UpdateLMCCWarnings());
            StartCoroutine(UpdateLMCCTodo());
            timeSinceLastUpdate = 0.0f;
        }
    }

    void OpenWarning()
    {
        warningText.gameObject.SetActive(true);
        warningDetailsText.gameObject.SetActive(true);
        warningVignette.gameObject.SetActive(true);
    }

    void CloseWarning()
    {
        warningText.gameObject.SetActive(false);
        warningDetailsText.gameObject.SetActive(false);
        warningVignette.gameObject.SetActive(false);
    }


    IEnumerator UpdateLMCCWarnings()
    {
        //Debug.Log("Updating Warnings");
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(lmccApiCallGetWarning))
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
                LMCCWarning lmccWarning = JsonConvert.DeserializeObject<LMCCWarning>(webRequest.downloadHandler.text);



                if (lmccWarning.infoWarning != "")
                {
                    //warningText.text = "Warning:";
                    warningDetailsText.text = lmccWarning.infoWarning;
                    OpenWarning();
                }
                else CloseWarning();
            }
        }
    }

    IEnumerator UpdateLMCCTodo()
    {
        //Debug.Log("Updating TODO");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(lmccApiCallGetTodo))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success) { Debug.LogError("Error: " + webRequest.error); }
            else
            {
                LMCCTodoItems lmccTodo = JsonConvert.DeserializeObject<LMCCTodoItems>(webRequest.downloadHandler.text);


                bool allDone = true;
                if (lmccTodo.todoItems != null)
                {
                    foreach (var todoItem in lmccTodo.todoItems)
                    {
                        if (todoItem[1] != "True")
                        {
                            allDone = false;
                            break;
                        }
                    }
                }


                // Larger todo screen
                // Again, look into optimizations later
                
                string newTodoList = "<indent=5%>";
                foreach (var todoItem in lmccTodo.todoItems)
                {
                    if (todoItem[1] != "True")
                    {
                        newTodoList += $"-  {todoItem[0]} \n";
                    }
                    else
                    {
                        newTodoList += $"- <s> {todoItem[0]} </s>\n";
                    }
                }
                messageDetailsText.text = newTodoList;

                if (newTodoList == "<indent=5%>")
                {
                    messageText.gameObject.SetActive(false);
                }
                else
                {
                    messageText.gameObject.SetActive(true);
                }
                
            }
        }
    }
}