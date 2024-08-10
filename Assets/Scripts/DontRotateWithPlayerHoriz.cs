using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRotateWithPlayerHoriz : MonoBehaviour
{
    private Player _player;
    private Quaternion _startRotation;
    
    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _startRotation = transform.localRotation;
    }

    public void Update()
    {
        if (Math.Abs(_player.transform.localScale.y - (-1)) < 0.001f)
        {
            transform.localRotation = _startRotation * Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.localRotation = _startRotation;
        }
    }
}
