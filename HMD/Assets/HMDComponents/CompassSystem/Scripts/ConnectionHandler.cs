using UnityEngine;


public class ConnectionHandler : MonoBehaviour
{
    // Start is called before the first frame update
<<<<<<< Updated upstream:HMD/Assets/HMDComponents/CompassSystem/Scripts/ConnectionHandler.cs
    public TSScConnection TSSc;
    void Start()
    {
        TSSc.ConnectToHost("172.24.196.222", 2, 14141);
=======
    public GatewayConnection gatewayConnection;
    void Start()
    {
        gatewayConnection.ConnectToHost("127.0.0.1", 2, 3001);
>>>>>>> Stashed changes:HMD/Assets/HMDComponents/networking/ConnectionHandler.cs
    }

    public TSScConnection GetConnection()
    {
        return this.gatewayConnection;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
