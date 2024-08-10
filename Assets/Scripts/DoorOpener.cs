using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public GameObject Button;
    public float doorSpeed = 1;
    private Button bt;
    private bool isMoving;
    private Vector3 openedPosition;
    private Vector3 closedPosition;
    private Vector3 tgPosition;
    
    void Start()
    {
        bt = Button.GetComponent<Button>();
        closedPosition = transform.position;
        openedPosition = transform.position + Vector3.up*3;
    }
    void Update(){
        if(bt.pressed && !isMoving){
            isMoving = true;
            tgPosition = openedPosition;
        }
        else if(!bt.pressed && !isMoving){
            isMoving = true;
            tgPosition = closedPosition;
        }
    }
    void FixedUpdate()
    {
        if(isMoving){
            if((tgPosition - transform.position).magnitude < Time.fixedDeltaTime * doorSpeed){
                isMoving = false;
                transform.position = tgPosition;
            }
            transform.position = (tgPosition - transform.position).normalized * doorSpeed * Time.fixedDeltaTime;
        }
    }
}
