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
    public void PlacePin(float x, float y, float z)
    {
        // summons a pin at the headset user's position, and adds pin to list of pins present in the current run
        GameObject newPin = Instantiate(worldPin,
                new Vector3(x, y, z),
                Quaternion.identity);
        allPins.Add(newPin);
    }
    public void PlacePin()
    {
        // summons a pin at the headset user's position, and adds pin to list of pins present in the current run
        PlacePin(playerPosition.x, playerPosition.y, playerPosition.z);
    }
}
