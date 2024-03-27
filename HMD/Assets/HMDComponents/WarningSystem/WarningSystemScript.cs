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
    public TMP_Text todoBody;
    public Canvas todoCanvas;

    [SerializeField] static readonly string lmccDeviceIp = "192.168.4.36";
    static readonly string lmccApiCallGetWarning = "http://" + lmccDeviceIp + ":3001/api/v0?get=warning";
    static readonly string lmccApiCallGetTodo = "http://" + lmccDeviceIp + ":3001/api/v0?get=todo";

    bool warningOccurring;
    bool updatingWarningsAndTodo;
    float timeSinceLastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastUpdate = 0.0f;
        warningOccurring = false;
        updatingWarningsAndTodo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (todoCanvas.gameObject.activeSelf)
            {
                todoCanvas.gameObject.SetActive(false);
            }
            else
            {
                todoCanvas.gameObject.SetActive(true);
            }
        }

        if (!updatingWarningsAndTodo)
        {
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate > 0.5f)
            {
                updatingWarningsAndTodo = true;
                StartCoroutine(UpdateLMCCWarnings());
                StartCoroutine(UpdateLMCCTodo());
                updatingWarningsAndTodo = false;
                timeSinceLastUpdate = 0.0f;
            }
        }

    }

    void OpenWarning()
    {
        warningOccurring = true;
        warningText.gameObject.SetActive(true);
        warningDetailsText.gameObject.SetActive(true);
        warningVignette.gameObject.SetActive(true);
    }

    void CloseWarning()
    {
        warningText.gameObject.SetActive(false);
        warningDetailsText.gameObject.SetActive(false);
        warningVignette.gameObject.SetActive(false);
        warningOccurring = false;
    }


    IEnumerator UpdateLMCCWarnings()
    {
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


                if (lmccTodo.todoItems == null || allDone)
                {  // This part is not optimized, but is designed to be readable
                    messageText.gameObject.SetActive(false);
                    messageDetailsText.gameObject.SetActive(false);
                }
                else
                {
                    messageText.gameObject.SetActive(true);
                    messageDetailsText.gameObject.SetActive(true);

                    //look into optimizing this next part
                    foreach (var todoItem in lmccTodo.todoItems)
                    {
                        if (todoItem[1] != "True")
                        {
                            messageDetailsText.text = todoItem[0];
                            break;
                        }
                    }
                }


                // Larger todo screen
                // Again, look into optimizations later
                if (lmccTodo.todoItems != null)
                {
                    string newTodoList = "<indent=5>\t";
                    foreach (var todoItem in lmccTodo.todoItems)
                    {
                        if (todoItem[1] != "True")
                        {
                            newTodoList += $"-  {todoItem[0]} \n\t";
                        }
                        else
                        {
                            newTodoList += $"- <s> {todoItem[0]} </s>\n\t";
                        }
                    }
                    todoBody.text = newTodoList;
                }
                else
                {
                    todoBody.text = "\n\tThere are no tasks on your task list. Check with LMCC for a new task!\n\t";
                }
            }
        }
    }
}