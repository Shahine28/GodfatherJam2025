using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class SunBehavior : MonoBehaviour
{
    private PlayerBattery _playerBattery;
    
    public UnityEvent OnPlayerDeathBySun;


    [Header("Battery Charge Settings")] 
    [SerializeField, NaughtyAttributes.ReadOnly] private int _currentBatterySlotsInSunRay;

    [SerializeField, Range(1, 10)] private float _timeToChargeOneBatterySlot = 1f;
    private float _chargeTimer;
    
    public UnityEvent OnSunStartCharging;
    public UnityEvent OnSunStopCharging;
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
            OnPlayerDeathBySun?.Invoke();
        }
        else if (other.gameObject.CompareTag("SolarPanel"))
        {
            Debug.Log("Recharge battery");
            if (_currentBatterySlotsInSunRay == 0)
            {
                OnSunStartCharging?.Invoke();
            }
            _currentBatterySlotsInSunRay++;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SolarPanel"))
        {
            _currentBatterySlotsInSunRay--;
            if (_currentBatterySlotsInSunRay == 0)
            {
                OnSunStopCharging?.Invoke();
            }
        }
    }

    private void Update()
    {
        if (_currentBatterySlotsInSunRay > 0)
        {
            _chargeTimer += Time.deltaTime;
            if (_chargeTimer >= _timeToChargeOneBatterySlot)
            {
                _chargeTimer = 0f;
                _playerBattery.RechargeBatterySlot();
            }
        }
        else if (_chargeTimer > 0f)
        {
            _chargeTimer = 0f;
        }
    }
}
