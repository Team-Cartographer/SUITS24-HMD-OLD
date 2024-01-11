using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class FollowPlayerTransform : MonoBehaviour
{
    Camera playerCamera;

    Vector3 playerPosition;
    Vector3 initialPosition;

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
        playerCamera = Camera.main;
        cameraYOffset = ignoreCameraYOffset ? 0 : Camera.main.GetComponentInParent<XROrigin>().CameraYOffset;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = playerCamera.transform.position;

        xIgnore = ignoreX ? 0 : 1;
        yIgnore = ignoreY ? 0 : 1;
        zIgnore = ignoreZ ? 0 : 1;

        transform.position = new Vector3(playerPosition.x * xIgnore, playerPosition.y * yIgnore - cameraYOffset, 
            playerPosition.z * zIgnore) + initialPosition;
    }
}
