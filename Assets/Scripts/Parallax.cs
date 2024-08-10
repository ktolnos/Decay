using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(-1f, 1f)] public float strength;
    public float moveToFgThreshold = 0f;
    
    private Camera _camera;
    private Vector3 _startCameraPosition;
    private Vector3 _startPosition;
    void Start()
    {
        _camera = Camera.main!;
        _startCameraPosition = _camera.transform.position;
        _startPosition = transform.position;

        if (strength < moveToFgThreshold) // -1 is fg; 0 is player; 1 is bg;
        {
            gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
        }
    }

    void Update()
    {
        var diff = _camera.transform.position - _startCameraPosition;
        transform.position = _startPosition + diff * strength;
    }
}
