using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject worldPin; // gets prefab for physical pin to be placed in the world
    [SerializeField] List<GameObject> allPins;

    Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void LateUpdate()
    {
        playerPosition = Camera.main.transform.position;

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            PlacePin(); // place pin with keyboard for testing purposes
        }
    }

    public void PlacePin()
    {
        // summons a pin at the headset user's position, and adds pin to list of pins present in the current run
        GameObject newPin = Instantiate(worldPin,
                new Vector3(playerPosition.x, playerPosition.y, playerPosition.z),
                Quaternion.identity);
        allPins.Add(newPin);
    }

    public void AddPinToLMCC()
    {
        /*LMCCPinsController lmccPinsController = FindObjectOfType<LMCCPinsController>();

        double[] latLongCoords = FindObjectOfType<LMCCPinsController>().ConvertMeterToLatLong(transform.position);

        string pinAsJson = "{ \"map\": \"add\", \"pins\": [\"" + latLongCoords[0] + "x" + latLongCoords[1] + "\"], \"dimensions\":[1] }";

        //StartCoroutine(lmccPinsController.AddPinFromHMD(pinAsJson));

        // eg. { "map": "add", "pins": ["49.494885734x99.475359834"], "dimensions":1 }*/
    }
}
