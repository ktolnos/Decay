using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Button : MonoBehaviour
{
    public bool pressed;
    public Color pressedColor = Color.green;
    public Color unpressedColor = Color.red;
    public Sprite pressedButton;
    public Sprite unpressedButton;
    private SpriteRenderer spriteRenderer;
    private Light2D light2D;
    private float lastStay = 0;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        pressed = true;
        spriteRenderer.sprite = pressedButton;
        spriteRenderer.color = pressedColor;
        light2D.color = pressedColor;
        lastStay = Time.time;
    }
    void FixedUpdate()
    {
        if(Time.time - lastStay > Time.fixedDeltaTime*5){
            pressed = false;
            spriteRenderer.sprite = unpressedButton;
            spriteRenderer.color = unpressedColor;
            light2D.color = unpressedColor;
        }
    }
}
