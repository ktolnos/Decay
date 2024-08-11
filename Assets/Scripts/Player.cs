using UnityEngine;

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
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip detachSound;
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
            DetachLeg();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DetachArm();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && hasGlider)
        {
            DetachGlider();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && hasTorso)
        {
            DetachTorso();
        }

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
                rightLegZ = Mathf.Clamp(rightLegZ, legMovementMinMax.x, legMovementMinMax.y);
            }
            rightLeg.transform.eulerAngles = new Vector3(0f, 0f, rightLegZ);
            if (hasLeftLeg)
            {
                leftLeg.transform.eulerAngles = new Vector3(0f, 0f, -rightLegZ);
            }
        }
        else if (hasRightArm)
        {
            rightArm.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            armMovementProgress += _currentHorizontalSpeed * armMovementSpeed * Time.deltaTime;
            if (Mathf.Abs(armMovementProgress) >= armMovementDistance)
            {
                armMovementSpeed *= -1;
                armMovementProgress = Mathf.Clamp(armMovementProgress, -armMovementDistance, armMovementDistance);
            }
            rightArm.transform.localPosition = rightArmLayPosition.localPosition + Vector3.up * armMovementProgress;
            if (armMovementSpeed > 0f && _currentHorizontalSpeed > 0f ||
                armMovementSpeed < 0f && _currentHorizontalSpeed < 0f)
            {
                _currentHorizontalSpeed = 0f;
            }
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
        hasGlider = false;
        Detach(glider);
        gliderCollider.gameObject.SetActive(false);
    }
    
    private void DetachTorso()
    {
        hasTorso = false;
        rb.AddForce(transform.up * torsoDetachForce * _scaleMult, ForceMode2D.Impulse);
        DetachLeftArm();
        DetachRightArm();
        DetachLeftLeg();
        DetachRightLeg();
        DetachGlider();
        Detach(torso);
        colliderNoLegs.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        wasntOnGround = !_isGrounded;
        if(Input.GetKey(KeyCode.LeftShift)){
            rb.gravityScale = 0;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            gliderEffect.SetActive(true);
        }else
        {
            rb.gravityScale = defGravityScale;
            gliderEffect.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DetachGlider();
        }
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
            audioSource.PlayOneShot(jumpSound);
            var jumpForce = hasLeftLeg ? jumpForce2Legs : hasRightLeg ? jumpForce1Leg : jumpForceNoLegs;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _startJump = false;
            DetachLeg();
        } else if (_startJump)
        {
            _startJumpDelayWaited += Time.fixedDeltaTime;
        }
        if (_startJumpDelayWaited >= startJumpDelay) _startJump = false;
        if (_isGrounded && wasntOnGround && hasRightLeg){
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
