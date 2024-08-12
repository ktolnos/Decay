using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed2Legs;
    public float speedGlider;
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
    public Rigidbody2D head;
    public Rigidbody2D leftLeg;
    public Rigidbody2D rightLeg;
    public Rigidbody2D leftArm;
    public Rigidbody2D rightArm;
    public Rigidbody2D torso;
    public Rigidbody2D glider;
    public Transform gliderCollider;
    public Transform collider2Legs;
    public Transform colliderNoLegs;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip detachSound;
    public AudioClip walkingSound;
    public AudioClip crawlingSound;
    public GameObject detachEffect;
    public GameObject landingEffect;
    public GameObject gliderEffect;
    public Vector2 legMovementMinMax = Vector2.one * 45;
    public float legMovementSpeed = 1f;
    public float rightLegMovementDir = 1f;
    private float rightLegZ;
    public float armMovementSpeed = 10f;
    public float armMovementDistance = 1f;
    public float armMovementProgress = 0f;
    public Transform rightArmLayPosition;
    
    private float _scaleMult = 1f;
    private AudioSource audioSource;
    private bool wasntOnGround;
    private float defGravityScale;
    public AnimationCurve landingScreenShake;
    public AnimationCurve detachScreenShake;
    public float screenShakeDuration = 0.2f;
    private CameraMover _cameraMover;
    private bool _usedGlider;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        defGravityScale = rb.gravityScale;
        gliderEffect.SetActive(false);
        _cameraMover = Camera.main.GetComponent<CameraMover>();
    }
    
    private void Update()
    {
        if(Time.timeScale == 0){
            return;
        }
        var horizontalInput = Input.GetAxis("Horizontal");
        if (hasTorso)
        {
            _scaleMult = horizontalInput > 0 ? 1 : horizontalInput < 0 ? -1 : _scaleMult;
        }
        transform.localScale = hasRightLeg ? new Vector3(_scaleMult, 1, 1) : new Vector3(1, _scaleMult, 1); 
        var speed = hasLeftLeg ? speed2Legs : hasRightLeg ? speed1Leg : hasRightArm ? speedNoLegs : 0f;
        _currentHorizontalSpeed = horizontalInput * speed;
        
        if ( Input.GetButtonDown("Jump") && (hasRightLeg || hasRightArm))
        {
            _startJump = true;
            _startJumpDelayWaited = 0;
        }

        if (Input.GetButtonDown("Fire1") && !hasLeftArm && hasTorso)
        {
            StartCoroutine(DetachTorso());
        }
        
        var isGliding = hasGlider && Input.GetKey(KeyCode.LeftShift);

        if (hasRightLeg)
        {
            if (Mathf.Abs(_currentHorizontalSpeed) > 0.01f && _isGrounded)
            {
                rightLegZ += _currentHorizontalSpeed * rightLegMovementDir * legMovementSpeed * Time.deltaTime;
            }
            else
            {
                if (Mathf.Abs(rightLegZ) > Time.deltaTime * legMovementSpeed)
                {
                    rightLegZ += rightLegZ > 0 ? -legMovementSpeed * Time.deltaTime : legMovementSpeed * Time.deltaTime;
                }
            }
            if (rightLegZ > legMovementMinMax.y || rightLegZ < legMovementMinMax.x)
            {
                rightLegMovementDir *= -1;
                audioSource.PlayOneShot(walkingSound);
                rightLegZ = Mathf.Clamp(rightLegZ, legMovementMinMax.x, legMovementMinMax.y);
            }
            rightLeg.transform.eulerAngles = new Vector3(0f, 0f, rightLegZ);
            if (hasLeftLeg)
            {
                leftLeg.transform.eulerAngles = new Vector3(0f, 0f, -rightLegZ);
            }
        }
        else if (hasRightArm && !isGliding)
        {
            rightArm.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            armMovementProgress += _currentHorizontalSpeed * armMovementSpeed * Time.deltaTime;
            if (Mathf.Abs(armMovementProgress) >= armMovementDistance)
            {
                armMovementSpeed *= -1;
                audioSource.PlayOneShot(crawlingSound);
                armMovementProgress = Mathf.Clamp(armMovementProgress, -armMovementDistance, armMovementDistance);
            }
            rightArm.transform.localPosition = rightArmLayPosition.localPosition + Vector3.up * armMovementProgress;
            if (_isGrounded && (armMovementSpeed > 0f && _currentHorizontalSpeed > 0f ||
                armMovementSpeed < 0f && _currentHorizontalSpeed < 0f))
            {
                _currentHorizontalSpeed = 0f;
            }
        }
        _currentHorizontalSpeed = isGliding ? speedGlider * horizontalInput : _currentHorizontalSpeed;

        if (!hasLeftArm && hasTorso)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            head.transform.right = _scaleMult * (mousePosition - head.transform.position);
        }

        if (!hasTorso)
        {
            rb.freezeRotation = false;
        }
    }

    private void DetachLeg()
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
    
    private void DetachArm()
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
    
    public void DetachLeftArm()
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
        if (!hasGlider)
        {
            return;
        }
        hasGlider = false;
        Detach(glider);
        gliderCollider.gameObject.SetActive(false);
    }
    
    private IEnumerator DetachTorso()
    {
        
        if (hasLeftArm)
        {
            DetachLeftArm();
            yield return 0;
        }
        if (hasRightArm)
        {
            DetachRightArm();
            yield return 0;
        }
        if (hasLeftLeg)
        {
            DetachLeftLeg();
            yield return 0;
        }
        if (hasRightLeg)
        {
            DetachRightLeg();
            yield return 0;
        }
        if (hasGlider)
        {
            DetachGlider();
            yield return 0;
        }
        
        hasTorso = false;
        Detach(torso);
        colliderNoLegs.gameObject.SetActive(false);
        var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction.z = 0;
        direction -= head.transform.position;
        direction.Normalize();
        rb.AddForce(direction * torsoDetachForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if( Time.timeScale == 0){
            return;
        }
        wasntOnGround = !_isGrounded;

        var usesGlider = hasGlider && Input.GetKey(KeyCode.LeftShift);
        if(usesGlider){
            rb.gravityScale = 0;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            gliderEffect.SetActive(true);
            gliderEffect.transform.up = Vector3.up;
            if (!_usedGlider)
            {
                transform.position += Vector3.up * 0.1f;
            }
            _usedGlider = true;
        }else
        {
            rb.gravityScale = defGravityScale;
            gliderEffect.SetActive(false);
            if (_usedGlider)
            {
                DetachGlider();
            }
        }
        if (!isGrappling && (hasRightLeg || hasRightArm) || usesGlider)
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
            audioSource.PlayOneShot(jumpSound);
            var jumpForce = hasLeftLeg ? jumpForce2Legs : hasRightLeg ? jumpForce1Leg : jumpForceNoLegs;
            var direction = hasRightLeg ? Vector3.up : transform.up * _scaleMult + Vector3.up;
            if (!hasRightLeg)
            {
                DetachRightArm();
            }
            else
            {
                DetachLeg();
            }
            rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
            _startJump = false;
        } else if (_startJump)
        {
            _startJumpDelayWaited += Time.fixedDeltaTime;
        }
        if (_startJumpDelayWaited >= startJumpDelay) _startJump = false;
        if (_isGrounded && wasntOnGround){
            audioSource.PlayOneShot(landingSound);
            _cameraMover.ScreenShake(screenShakeDuration, landingScreenShake);
            Destroy(Instantiate(landingEffect, transform), 1);
        }
    }
    
    private void Detach(Rigidbody2D part)
    {
        Destroy(Instantiate(detachEffect, transform), 1);
        audioSource.PlayOneShot(detachSound);
        part.simulated = true;
        part.transform.parent = null;
        part.velocity = Vector2.zero;
        _cameraMover.ScreenShake(screenShakeDuration, detachScreenShake);
    }
}
