using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public GameObject player;
    public float grapplingHookWeight = 0.5f;
    private Vector3 newCameraPosition;
    private Vector3 coursorPosition;
    private void Update() {
        coursorPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        newCameraPosition = player.transform.position + coursorPosition*grapplingHookWeight;
        transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, transform.position.z);
    }
}
