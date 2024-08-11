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
    private Rigidbody2D _rb;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _armLength = Vector2.Distance(arm.transform.position, transform.position);
        _player = GetComponentInParent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Update()    
    {
        if(Time.timeScale == 0){
            return;
        }
        if (!_player.hasLeftArm)
        {
            arm.rb.gravityScale = 1f;
            arm.rb.simulated = true;
            arm.rb.isKinematic = false;
            arm.enabled = false;
            arm.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            
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
            if (detachAfterUse && _shotTime != 0f)
            {
                _player.DetachLeftArm();
            }
        }

        if (arm.state == GrapplingArm.State.Attached && Time.time - _shotTime > maxTime)
        {
            arm.Detach();
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
            }
            else
            {
                _player.rb.velocity = (arm.transform.position - transform.position) / distance * attractionSpeed;
            }
        }

        if (distance > maxDistance)
        {
            arm.Detach();
        }
    }
}