using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Compass : MonoBehaviour
{

    // Start is called before the first frame update
    public TSScConnection TSSc;

    void Start()
    {
        TSSc.ConnectToHost("172.24.196.222", 2, 14141);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
