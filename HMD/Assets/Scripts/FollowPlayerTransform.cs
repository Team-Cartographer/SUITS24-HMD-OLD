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
    //Quaternion initialRotation;

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

    /*[Header("Rotation")]
    [SerializeField] bool rotateX = false;
    [SerializeField] bool rotateY = false;
    [SerializeField] bool rotateZ = false;
    int xRotate;
    int yRotate;
    int zRotate;*/

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        //initialRotation = transform.rotation;
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = playerCamera.transform.position;

        cameraYOffset = ignoreCameraYOffset ? 0 : playerCamera.GetComponentInParent<XROrigin>().CameraYOffset;
        xIgnore = ignoreX ? 0 : 1;
        yIgnore = ignoreY ? 0 : 1;
        zIgnore = ignoreZ ? 0 : 1;

        transform.position = new Vector3(playerPosition.x * xIgnore, playerPosition.y * yIgnore - cameraYOffset, 
            playerPosition.z * zIgnore) + initialPosition;

        /*xRotate = rotateX ? 1 : 0;
        yRotate = rotateY ? 1 : 0;
        zRotate = rotateZ ? 1 : 0;
        transform.rotation = new Quaternion(playerCamera.transform.rotation.x + initialRotation.x,
            playerCamera.transform.rotation.y + initialRotation.y,
            playerCamera.transform.rotation.z + initialRotation.z,
            playerCamera.transform.rotation.w + initialRotation.w);*/
    }
}
