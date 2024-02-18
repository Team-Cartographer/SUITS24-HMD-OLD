using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapFollow : MonoBehaviour
{
    LazyFollow lazyFollow;
    // Start is called before the first frame update
    void Start()
    {
        lazyFollow = GetComponent<LazyFollow>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
