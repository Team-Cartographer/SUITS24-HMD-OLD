using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class voiceCenter : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    public Canvas telemetryCanvas;

    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(new string[] { 
            "on", "off" });
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Voice command: " + args.text);

        if (args.text == "on")
        {
            telemetryCanvas.gameObject.SetActive(true);
        }
        else if (args.text == "off")
        {
            telemetryCanvas.gameObject.SetActive(false);
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