using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class voiceCenter : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    // telemetry
    public Canvas telemetryCanvas;

    // geosampling
    public Canvas geoCanvas; 

    // active time
    public Canvas timeCanvas;

    // waring and todo list
    public TMP_Text warningText;
    public TMP_Text warningDetailsText;
    public TMP_Text messageText;
    public TMP_Text messageDetailsText;
    public RawImage warningVignette;


    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(new string[] { 
            // telemetry
            "telemetry on", "telemtery off",
            // geosampling
            "geo on", "geo off",
            // timer
            "time on", "time off",
            // todo
            "to do on", "to do off",
            // warning
            "warning on", "warning off"
        });
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Voice command: " + args.text);

        // telemetry
        if (args.text == "telemtery on")
        {
            telemetryCanvas.gameObject.SetActive(true);
        }
        else if (args.text == "telemetry off")
        {
            telemetryCanvas.gameObject.SetActive(false);
        }
        // geo sampling
        else if (args.text == "geo on")
        {
            geoCanvas.gameObject.SetActive(true);
        }
        else if (args.text =="geo off")
        {
            geoCanvas.gameObject.SetActive(false);
        }
        // timer
        else if (args.text =="timer on")
        {
            timeCanvas.gameObject.SetActive(true);
        }
        else if (args.text =="timer off")
        {
            timeCanvas.gameObject.SetActive(false);
        }
        // todo and warning
        else if (args.text =="to do on")
        {
            messageText.gameObject.SetActive(true);
            messageDetailsText.gameObject.SetActive(true);
        }
        else if (args.text == "to do off")
        {
            messageText.gameObject.SetActive(false);
            messageDetailsText.gameObject.SetActive(false);
        }
        else if (args.text == "warning on")
        {
            warningVignette.gameObject.SetActive(true);
            warningText.gameObject.SetActive(true);
            warningDetailsText.gameObject.SetActive(true);
        }
        else if (args.text == "warning off")
        {
            warningVignette.gameObject.SetActive(false);
            warningText.gameObject.SetActive(false);
            warningDetailsText.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}