using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRotateWithPlayerHoriz : MonoBehaviour
{
    private Player _player;
    
    private void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    public void Update()
    {
        transform.localScale = _player.transform.localScale;
    }
}
