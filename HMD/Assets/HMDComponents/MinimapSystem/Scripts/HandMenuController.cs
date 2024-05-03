using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandMenuController : MonoBehaviour // basic scene managing script
{
    [SerializeField] ConnectionHandler connectionHandler;
    [SerializeField] TMP_Text setIPText;
    [SerializeField] TMP_Text currentIPText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentIPText.text = "Current IP:\n" + PlayerPrefs.GetString("CurrentIP"); // default: 10.34.69.33
    }

    public void QuitApplication() // quit app button action
    {
        Application.Quit();
    }

    public void SetIP() // add ip button action
    {
        SetIP(setIPText.text);
    }

    public void SetIP(string ip) // add ip button action
    {
        if (ip == "Type an IP" || ip.Length < 7) return;
        PlayerPrefs.SetString("CurrentIP", ip);
        connectionHandler.SetHostIP(PlayerPrefs.GetString("CurrentIP"));
        setIPText.text = "Type an IP";
    }

    public void NumpadInput(string input) // numpad button action
    {
        if (setIPText.text == "Type an IP") setIPText.text = "";
        if (input == "backspace") { if (setIPText.text.Length > 0) setIPText.text = setIPText.text.Substring(0, setIPText.text.Length - 1); }
        else setIPText.text += input;
        if (setIPText.text.Length == 0) setIPText.text = "Type an IP";
    }

    public void ShowCurrentIP()
    {

    }

    /*public void AddIP()
    {
        
    }

    public void DeleteIPs()
    {
    
    }*/
}
