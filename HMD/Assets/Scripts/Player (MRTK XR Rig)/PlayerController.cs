using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject worldPin;
    [SerializeField] List<GameObject> allPins;

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
        Vector3 playerPosition = Camera.main.transform.position;

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            GameObject newPin = Instantiate(worldPin,
                new Vector3(playerPosition.x, playerPosition.y, playerPosition.z),
                Quaternion.identity);
            allPins.Add(newPin);
        }
    }
}
