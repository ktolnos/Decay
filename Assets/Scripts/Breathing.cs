using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public float transformStrength;
    public float scaleStrength;
    private Vector3 _initialLocalPosition;
    private Vector3 _initialScale;
    

    private void Start()
    {
        _initialLocalPosition = transform.localPosition;
        _initialScale = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = _initialScale + Vector3.one * Mathf.Sin(Time.time * 5) * scaleStrength;
        transform.localPosition = _initialLocalPosition + Vector3.up * Mathf.Sin(Time.time * 5) * transformStrength;
    }
}
