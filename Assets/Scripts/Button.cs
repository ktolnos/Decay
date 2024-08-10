using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool pressed;
    public Sprite pressedButton;
    public Sprite unpressedButton;
    private SpriteRenderer spriteRenderer;
    private float lastStay = 0;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        pressed = true;
        spriteRenderer.sprite = pressedButton;
        lastStay = Time.time;
    }
    void FixedUpdate()
    {
        if(Time.time - lastStay > Time.fixedDeltaTime*5){
            pressed = false;
            spriteRenderer.sprite = unpressedButton;
        }
    }
}
