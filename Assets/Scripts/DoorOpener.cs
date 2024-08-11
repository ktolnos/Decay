using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public GameObject Button;
    public float doorSpeed = 1;
    private Button bt;
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
        if(bt.pressed){
            tgPosition = openedPosition;
        }
        else if(!bt.pressed){
            tgPosition = closedPosition;
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
