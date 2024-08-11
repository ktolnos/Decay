using System;
using UnityEngine;

public class GrapplingBase: MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public GrapplingArm arm;
    private float _armLength;
    public float attractionSpeed = 1f;
    private Player _player;
    public float minDistance;
    public float maxDistance;
    public float maxTime = 1f;
    private float _shotTime;
    public Transform lineStart;
    public Transform lineEnd;
    public bool detachAfterUse = true;
    private bool _wasAttached;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _armLength = Vector2.Distance(arm.transform.position, transform.position);
        _player = GetComponentInParent<Player>();
    }

    public void Update()    
    {
        if(Time.timeScale == 0){
            return;
        }
        if (!_player.hasLeftArm)
        {
            arm.rb.gravityScale = 10f;
            arm.rb.simulated = true;
            arm.rb.isKinematic = false;
            arm.enabled = false;
            
            _lineRenderer.SetPosition(0, lineStart.position);
            _lineRenderer.SetPosition(1, lineEnd.position);
            return;
        }
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
            if (detachAfterUse && _wasAttached)
            {
                _player.DetachLeftArm();
            }
        }

        if (arm.state == GrapplingArm.State.Attached)
        {
            _wasAttached = true;
            if (Time.time - _shotTime > maxTime)
            {
                arm.Detach();
                Debug.Log("Detach by time");
            }
        }
        _player.isGrappling = arm.state == GrapplingArm.State.Attached;

        arm.transform.rotation = transform.rotation;
        
        if (Input.GetButtonDown("Fire1"))
        {
            _shotTime = Time.time;
            arm.Shoot();
        }
        _lineRenderer.SetPosition(0, lineStart.position);
        _lineRenderer.SetPosition(1, lineEnd.position);
    }

    private void FixedUpdate()
    {
        if(Time.timeScale == 0){
            return;
        }
        if (!_player.hasLeftArm)
        {
            return;
        }
        var distance = Vector2.Distance(arm.transform.position, transform.position);
        if (arm.state == GrapplingArm.State.Attached)
        {
            if (distance < minDistance)
            {
                arm.Detach();
                Debug.Log("Detach by min distance");
            }
            else
            {
                _player.rb.velocity = (arm.transform.position - transform.position) / distance * attractionSpeed;
            }
        }

        if (distance > maxDistance && !_wasAttached)
        {
            arm.Detach();
            Debug.Log("Detach by max distance");
        }
    }
}