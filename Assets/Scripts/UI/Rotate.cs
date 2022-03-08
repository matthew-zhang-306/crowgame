using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private RectTransform skyRectTransform;
    private RectTransform triRectTransform;
    private Transform Telescope;
    Vector3 skyPos;
    Vector3 triPos;
    void Start()
    {
        Telescope = this.GetComponent<Transform>();
        Transform animatedSky = Telescope.GetChild(0);
        Transform animatedTriangle = Telescope.GetChild(1);
        skyRectTransform = animatedSky.GetComponent<RectTransform>();
        triRectTransform = animatedTriangle.GetComponent<RectTransform>();

        skyPos = skyRectTransform.rect.center;
        skyPos = skyRectTransform.TransformPoint(skyPos);
        triPos = triRectTransform.rect.center;
        triPos = triRectTransform.TransformPoint(triPos);
    }
    void Update()
    {
        skyRectTransform.RotateAround(skyPos, Vector3.forward, 0.1f);
        triRectTransform.RotateAround(triPos, Vector3.forward, -0.1f);
    }
}
