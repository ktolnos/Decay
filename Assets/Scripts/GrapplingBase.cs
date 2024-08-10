using System;
using UnityEngine;

public class GrapplingBase: MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public GrapplingArm arm;
    private float _armLength;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _armLength = Vector2.Distance(arm.transform.position, transform.position);
    }

    public void Update()
    {
        _lineRenderer.SetPosition(1, transform.position - arm.transform.position);
        if (arm.state != GrapplingArm.State.Idle)
        {
            transform.up = arm.transform.position - transform.position;
        }
        else
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.up = mousePosition - transform.position;
        }

        if (arm.state == GrapplingArm.State.Idle)
        {
            arm.transform.position = transform.up * _armLength + transform.position;
        }

        arm.transform.rotation = transform.rotation;
        
        if (Input.GetButtonDown("Fire1"))
        {
            arm.Shoot();
        }

        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            arm.Detach();
        }
    }
}