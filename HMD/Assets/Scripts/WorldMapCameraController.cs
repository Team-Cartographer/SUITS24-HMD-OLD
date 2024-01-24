using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapCameraController : MonoBehaviour
{
    [SerializeField] RectTransform scrollContent;
    [SerializeField] MixedReality.Toolkit.UX.Slider sizeSlider;
    float scrollConstant = 0.006875f;

    Camera mapCamera;

    // Start is called before the first frame update
    void Start()
    {
        sizeSlider.Value = 2;
        mapCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float scrollMultiplier = scrollConstant * sizeSlider.Value;
        float newX = -scrollContent.anchoredPosition.x * scrollMultiplier;
        float newZ = -scrollContent.anchoredPosition.y * scrollMultiplier;
        transform.position = new Vector3(newX, transform.position.y, newZ);
        mapCamera.orthographicSize = sizeSlider.Value;
    }
}
