using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject worldPin;
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
            PlacePin();
        }
    }

    public void PlacePin()
    {
        GameObject newPin = Instantiate(worldPin,
                new Vector3(playerPosition.x, playerPosition.y, playerPosition.z),
                Quaternion.identity);
        allPins.Add(newPin);
    }
}
