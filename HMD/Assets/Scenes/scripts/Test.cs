using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public TSScConnection TSSc;

    // Start is called before the first frame update
    void Start()
    {
        TSSc = new TSScConnection();
        TSSc.ConnectToHost("172.24.196.222", 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(TSSc.GetTELEMETRYJsonString());
    }
}
