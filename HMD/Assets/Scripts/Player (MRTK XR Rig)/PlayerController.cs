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
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            allPins.Add(Instantiate(worldPin, 
                new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                Quaternion.identity));
        }
    }
}
