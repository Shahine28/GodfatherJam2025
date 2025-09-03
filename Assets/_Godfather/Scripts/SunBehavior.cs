using System;
using UnityEngine;

public class SunBehavior : MonoBehaviour
{
    private PlayerBattery _playerBattery;
    
    private void Start()
    {
        _playerBattery = FindFirstObjectByType<PlayerBattery>();
        if (_playerBattery == null)
        {
            Debug.LogError("PlayerBattery not found in the scene.");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Death by sun");
        }
        else if (other.gameObject.CompareTag("SolarPanel"))
        {
            Debug.Log("Recharge battery");
            _playerBattery.RechargeBatterySlot();
        }
    }
}
