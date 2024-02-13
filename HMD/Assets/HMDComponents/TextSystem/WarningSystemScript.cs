using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WarningSystemScript : MonoBehaviour
{
    public TMP_Text warningText;
    public TMP_Text warningDetailsText;
    public RawImage warningVignette;

    bool warningOccurring;


    /*public static async String[] ApiCaller
    {
        Debug.Log("Hello world");
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
            if (!warningOccurring) StartWarning();
            else EndWarning();
        }
    }

    void StartWarning(){
        warningOccurring = true;
        warningText.gameObject.SetActive(true);
        warningDetailsText.gameObject.SetActive(true);
        warningVignette.gameObject.SetActive(true);
    }

    void EndWarning(){
        warningText.gameObject.SetActive(false);
        warningDetailsText.gameObject.SetActive(false);
        warningVignette.gameObject.SetActive(false);
        warningOccurring = false;
    }
}
