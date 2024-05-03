using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapCameraController : MonoBehaviour
{
    [SerializeField] RectTransform scrollContent; // scrolling area that camera position will be referencing
    [SerializeField] MixedReality.Toolkit.UX.Slider sizeSlider;
    float scrollConstant = 0.006875f; // constant scale value determined through trial and error

    Camera mapCamera; // camera floating above player; only renders the map and map icons

    // Start is called before the first frame update
    void Start()
    {
        sizeSlider.Value = sizeSlider.MinValue;
        mapCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // sets position of camera based on position of scroll area content and scale
        float scrollMultiplier = scrollConstant * sizeSlider.Value;
        float newX = -scrollContent.anchoredPosition.x * scrollMultiplier;
        float newZ = -scrollContent.anchoredPosition.y * scrollMultiplier;
        transform.position = new Vector3(newX, transform.position.y, newZ);
        mapCamera.orthographicSize = sizeSlider.Value;
    }
}
