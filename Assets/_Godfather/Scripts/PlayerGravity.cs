using System;
using UnityEngine;

public class PlayerGravity : PlanetGravity
{
    private PlayerJump _playerJump;

    private void Awake()
    {
        _playerJump = GetComponent<PlayerJump>();
        if (_playerJump == null)
            Debug.LogWarning("[PlayerGravity] Le Player doit avoir un composant PlayerJump.");
    }

    protected override  void SimulateGravity() 
    {
        if (_playerJump.IsGrounded) return;
        base.SimulateGravity();
    }
}
