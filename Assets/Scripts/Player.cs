using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    private float _currentHorizontalSpeed;
    private bool _startJump;
    private bool _isGrounded;
    public Transform[] groundChecks;
    public float groundCheckLength;
    public float startJumpDelay = 0.2f;
    private float _startJumpDelayWaited;
    public bool isGrappling;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        transform.localScale = horizontalInput > 0 ? new Vector3(1, 1, 1)
            : horizontalInput < 0 ? new Vector3(-1, 1, 1) : transform.localScale;
        _currentHorizontalSpeed = horizontalInput * speed;
        if ( Input.GetButtonDown("Jump"))
        {
            _startJump = true;
            _startJumpDelayWaited = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!isGrappling)
        {
            rb.velocity = new Vector2(_currentHorizontalSpeed, rb.velocity.y);
        }
        _isGrounded = false;
        foreach (var groundCheck in groundChecks)
        {
            _isGrounded |= Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckLength,
                LayerMask.GetMask("Ground"));
        }
        if (_isGrounded && _startJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _startJump = false;
            _isGrounded = false;
        } else if (_startJump)
        {
            _startJumpDelayWaited += Time.fixedDeltaTime;
        }
        if (_startJumpDelayWaited >= startJumpDelay) _startJump = false;
    }
}
