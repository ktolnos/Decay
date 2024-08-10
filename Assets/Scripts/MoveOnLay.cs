using System;
using UnityEngine;

public class MoveOnLay: MonoBehaviour
{
    private Player _player;
    public Transform target;
    private bool _moved;
    
    private void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    public void Update()
    {
        if (!_moved && !_player.hasRightLeg)
        {
            transform.localPosition = target.localPosition;
            _moved = true;
            enabled = false;
        }
    }
}