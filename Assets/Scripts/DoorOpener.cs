using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorOpener : MonoBehaviour
{
    public GameObject Button;
    public Button Button2;
    public float doorSpeed = 1;
    private Color closedColor;
    private Color openedColor;
    private Button bt;
    private Vector3 openedPosition;
    private Vector3 closedPosition;
    private Vector3 tgPosition;
    private SpriteRenderer spriteRenderer;
    private Light2D light2D;
    private bool _hasSecondButton;
    
    void Start()
    {
        bt = Button.GetComponent<Button>();
        closedPosition = transform.position;
        openedPosition = transform.position + Vector3.up*3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        openedColor = bt.pressedColor;
        closedColor = bt.unpressedColor;
        light2D = GetComponent<Light2D>();
        _hasSecondButton = Button2 != null;
    }
    void Update(){
        if(bt.pressed || (_hasSecondButton && Button2.pressed)){
            tgPosition = openedPosition;
            spriteRenderer.color = openedColor;
            light2D.color = openedColor;
        }
        else {
            tgPosition = closedPosition;
            spriteRenderer.color = closedColor;
            light2D.color = closedColor;
        }
    }
    void FixedUpdate()
    {
        if((tgPosition - transform.position).magnitude < Time.fixedDeltaTime * doorSpeed){
            transform.position = tgPosition;
        }
        transform.position += (tgPosition - transform.position).normalized * doorSpeed * Time.fixedDeltaTime;
    }
}
