using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Compass : MonoBehaviour
{

    // Start is called before the first frame update
    public ConnectionHandler connectionHandler;
    void Start()
    {
        Debug.Log(connectionHandler.GetConnection());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
