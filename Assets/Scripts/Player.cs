using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed;
    public float jumpForce;
    private float _currentHorizontalSpeed;
    private bool _startJump;
    private bool _isGrounded;
    public Transform[] groundChecks;
    public float groundCheckLength;
    public float startJumpDelay = 0.2f;
    private float _startJumpDelayWaited;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        _rb.velocity = new Vector2(_currentHorizontalSpeed, _rb.velocity.y);
        _isGrounded = false;
        foreach (var groundCheck in groundChecks)
        {
            _isGrounded |= Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckLength,
                LayerMask.GetMask("Ground"));
        }
        if (_isGrounded && _startJump)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _startJump = false;
            _isGrounded = false;
        } else if (_startJump)
        {
            _startJumpDelayWaited += Time.fixedDeltaTime;
        }
        if (_startJumpDelayWaited >= startJumpDelay) _startJump = false;
    }
}
