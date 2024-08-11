using System;
using UnityEngine;

public class GrapplingArm: MonoBehaviour
{
    public Rigidbody2D rb;
    public float force;
    public State state = State.Idle;
    public Vector3 attachedPosition;

    public AudioClip hookAttachSound;
    public AudioClip hookDetachSound;

    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (state != State.Idle)
        {
            return;
        }
        rb.simulated = true;
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * force, ForceMode2D.Impulse);
        state = State.Flying;
        audioSource.PlayOneShot(hookAttachSound);
    }

    public void Detach()
    {
        if (state == State.Attached)
        {
            audioSource.PlayOneShot(hookDetachSound);
        }
        rb.simulated = false;
        state = State.Idle;
    }

    private void FixedUpdate()
    {
        if (state == State.Attached)
        {
            rb.velocity = Vector2.zero;
            transform.position = attachedPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (state == State.Flying && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.isKinematic = true;
            rb.simulated = true;
            state = State.Attached;
            attachedPosition = other.contacts[0].point;
            transform.position = attachedPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Damaging"))
        {
            Detach();
        }
    }

    public enum State
    {
        Attached,
        Idle,
        Flying
    }
}