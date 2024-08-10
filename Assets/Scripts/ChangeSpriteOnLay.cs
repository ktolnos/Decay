using System;
using UnityEngine;

public class ChangeSpriteOnLay: MonoBehaviour
{
    private Player _player;
    private SpriteRenderer _spriteRenderer;
    public Sprite sprite;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GetComponentInParent<Player>();
    }

    public void Update()
    {
        if (!_player.hasRightLeg)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}