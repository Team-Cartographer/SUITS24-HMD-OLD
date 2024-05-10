using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIconScaling : MonoBehaviour
{
    Slider slider;
    float baseTransformScale;
    float currentScale;

    // Start is called before the first frame update
    void Start()
    {
        baseTransformScale = transform.localScale.x;
        slider = FindObjectOfType<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider) currentScale = slider.Value;
        else if (!slider) slider = FindObjectOfType<Slider>();
        transform.localScale = new Vector3(baseTransformScale, baseTransformScale, baseTransformScale) * currentScale;
    }
}
