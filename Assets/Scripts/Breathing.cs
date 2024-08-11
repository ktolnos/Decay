using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public float transformStrength;
    public float scaleStrength;
    public float rotationStrength;
    private Vector3 _initialLocalPosition;
    private Vector3 _initialScale;
    private Quaternion _initialRotation;
    public float breathingSpeed = 5f;
    

    private void Start()
    {
        _initialLocalPosition = transform.localPosition;
        _initialScale = transform.localScale;
        _initialRotation = transform.localRotation;
    }

    private void Update()
    {
        transform.localScale = _initialScale + Vector3.one * Mathf.Sin(Time.time * breathingSpeed) * scaleStrength;
        transform.localPosition = _initialLocalPosition + Vector3.up * Mathf.Sin(Time.time * breathingSpeed) * transformStrength;
        transform.localRotation = _initialRotation * Quaternion.Euler(0, 0, Mathf.Sin(Time.time * breathingSpeed) * rotationStrength);
    }
}
