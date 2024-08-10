using System;
using UnityEngine;

public class GrapplingArm: MonoBehaviour
{
    public Rigidbody2D rb;
    public float force;
    public State state = State.Idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot()
    {
        rb.simulated = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.forward * force, ForceMode2D.Impulse);
        state = State.Flying;
    }

    public void Detach()
    {
        rb.simulated = false;
        state = State.Idle;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (state == State.Flying && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = Vector2.zero;
            state = State.Attached;
        }
    }
    
    public enum State
    {
        Attached,
        Idle,
        Flying
    }
}