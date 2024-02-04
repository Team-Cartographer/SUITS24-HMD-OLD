using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class FollowPlayerTransform : MonoBehaviour // may delete this script
{
    Camera playerCamera;

    Vector3 playerPosition;
    Vector3 initialPosition;

    // settings for following player (rotate with player, follow player x/y/z, etc.)
    [Header("Camera Offset")]
    [SerializeField] bool ignoreCameraYOffset = true;
    float cameraYOffset;

    [Header("Ignore Directions")]
    [SerializeField] bool ignoreX = false;
    [SerializeField] bool ignoreY = false;
    [SerializeField] bool ignoreZ = false;
    int xIgnore;
    int yIgnore;
    int zIgnore;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = playerCamera.transform.position;

        // sets movement based on whether bools have been set true or not for the object the script's attached to
        cameraYOffset = ignoreCameraYOffset ? 0 : playerCamera.GetComponentInParent<XROrigin>().CameraYOffset;
        xIgnore = ignoreX ? 0 : 1;
        yIgnore = ignoreY ? 0 : 1;
        zIgnore = ignoreZ ? 0 : 1;

        transform.position = new Vector3(playerPosition.x * xIgnore, playerPosition.y * yIgnore - cameraYOffset, 
            playerPosition.z * zIgnore) + initialPosition;
    }
}
