using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class WorldMapFollow : MonoBehaviour
{
    LazyFollow lazyFollow;
    bool isInCourotine;

    // Start is called before the first frame update
    void Start()
    {
        lazyFollow = GetComponent<LazyFollow>();
        isInCourotine = false;
        if (!isInCourotine) StartCoroutine(RotateToPlayerThenFixPos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() 
    {
        lazyFollow = GetComponent<LazyFollow>();
        isInCourotine = false;
        if (!isInCourotine) StartCoroutine(RotateToPlayerThenFixPos());
    }

    IEnumerator RotateToPlayerThenFixPos()
    {
        isInCourotine = true;
        lazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
        yield return new WaitForFixedUpdate();
        lazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.None;
        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        isInCourotine = false;
    }
}
