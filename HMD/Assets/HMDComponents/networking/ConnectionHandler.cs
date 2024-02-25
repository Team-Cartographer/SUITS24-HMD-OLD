using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ConnectionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GatewayConnection TSSc;
    void Start()
    {
        TSSc.ConnectToHost("127.0.0.1", 2, 3001);
    }

    public GatewayConnection GetConnection()
    {
        return this.TSSc;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
