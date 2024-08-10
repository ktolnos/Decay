using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed2Legs;
    public float speed1Leg;
    public float speedNoLegs;
    public float jumpForce2Legs;
    public float jumpForce1Leg;
    public float jumpForceNoLegs;
    public float torsoDetachForce;
    private float _currentHorizontalSpeed;
    private bool _startJump;
    private bool _isGrounded;
    public Transform[] groundChecks;
    public float groundCheckLength;
    public float startJumpDelay = 0.2f;
    private float _startJumpDelayWaited;
    public bool isGrappling;
    public bool hasLeftLeg = true;
    public bool hasRightLeg = true;
    public bool hasLeftArm = true;
    public bool hasRightArm = true;
    public bool hasGlider = true;
    public bool hasTorso = true;
    public Rigidbody2D leftLeg;
    public Rigidbody2D rightLeg;
    public Rigidbody2D leftArm;
    public Rigidbody2D rightArm;
    public Rigidbody2D torso;
    public Rigidbody2D glider;
    public Transform gliderCollider;
    public Transform collider2Legs;
    public Transform colliderNoLegs;
    
    private float _scaleMult = 1f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        if (hasTorso)
        {
            _scaleMult = horizontalInput > 0 ? 1 : horizontalInput < 0 ? -1 : _scaleMult;
        }
        transform.localScale = hasRightLeg ? new Vector3(_scaleMult, 1, 1) : new Vector3(1, _scaleMult, 1); 
        var speed = hasLeftLeg ? speed2Legs : hasRightLeg ? speed1Leg : hasRightArm ? speedNoLegs : 0f;
        _currentHorizontalSpeed = horizontalInput * speed;
        if ( Input.GetButtonDown("Jump"))
        {
            _startJump = true;
            _startJumpDelayWaited = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (hasLeftLeg)
            {
                DetachLeftLeg();
            }
            else if (hasRightLeg)
            {
                DetachRightLeg();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hasLeftArm)
            {
                DetachLeftArm();
            }
            else if (hasRightArm)
            {
                DetachRightArm();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && hasGlider)
        {
            DetachGlider();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && hasTorso)
        {
            DetachTorso();
        }
    }
    
    private void DetachLeftArm()
    {
        hasLeftArm = false;
        Detach(leftArm);
    }
    
    private void DetachRightArm()
    {
        hasRightArm = false;
        Detach(rightArm);
    }
    
    private void DetachLeftLeg()
    {
        hasLeftLeg = false;
        Detach(leftLeg);
    }
    
    private void DetachRightLeg()
    {
        hasRightLeg = false;
        Detach(rightLeg);
        transform.up = Vector3.right;
        collider2Legs.gameObject.SetActive(false);
        colliderNoLegs.gameObject.SetActive(true);
    }
    
    private void DetachGlider()
    {
        hasGlider = false;
        Detach(glider);
        gliderCollider.gameObject.SetActive(false);
    }
    
    private void DetachTorso()
    {
        hasTorso = false;
        DetachLeftArm();
        DetachRightArm();
        DetachLeftLeg();
        DetachRightLeg();
        DetachGlider();
        Detach(torso);
        colliderNoLegs.gameObject.SetActive(false);
        rb.AddForce(transform.up * torsoDetachForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (!isGrappling && (hasRightLeg || hasRightArm))
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
            var jumpForce = hasLeftLeg ? jumpForce2Legs : hasRightLeg ? jumpForce1Leg : jumpForceNoLegs;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _startJump = false;
            _isGrounded = false;
        } else if (_startJump)
        {
            _startJumpDelayWaited += Time.fixedDeltaTime;
        }
        if (_startJumpDelayWaited >= startJumpDelay) _startJump = false;
    }
    
    private void Detach(Rigidbody2D part)
    {
        part.simulated = true;
        part.transform.parent = null;
    }
}
