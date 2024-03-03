using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;


public class Compass : MonoBehaviour
{
    public RawImage CompassImage;

    // Start is called before the first frame update
    public ConnectionHandler connectionHandler;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float cameraYaw = Camera.main.transform.rotation.eulerAngles.y;
        float cameraOffset = 0;

        GatewayConnection conn = connectionHandler.GetConnection();
        if (conn != null && conn.isIMUUpdated())
        {

            string IMUstring = conn.GetIMUJsonString();

            // Load IMU data into map
            JObject jo = JObject.Parse(IMUstring);
            float tssHeading = jo["imu"]["eva1"]["heading"].ToObject<float>();

            cameraOffset = cameraYaw - tssHeading;
        }
        
        // Update with tss server/prevent desync
        cameraYaw -= cameraOffset;

        // Rotate image
        CompassImage.uvRect = new Rect(cameraYaw / 360, 0, 1, 1);
    }
}
