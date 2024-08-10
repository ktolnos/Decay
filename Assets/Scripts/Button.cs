using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool pressed;
    public GameObject Door;
    public Sprite pressedButton;
    public Sprite unpressedButton;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionStay(Collision other)
    {
        pressed = true;
        spriteRenderer.sprite = pressedButton;

    }
    void OnCollisionExit(Collision other)
    {
        pressed = false;
        spriteRenderer.sprite = unpressedButton;
    }
    
}
