using System;
using System.Collections.Generic;
using UnityEngine;


public class ConnectionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GatewayConnection GatewayConnection;
    void Start()
    {
        GatewayConnection.ConnectToHost("192.168.4.36", 3001);
    }

    public GatewayConnection GetConnection()
    {
        return this.GatewayConnection;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
