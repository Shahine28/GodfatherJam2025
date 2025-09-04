using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBattery : MonoBehaviour
{
    [SerializeField] private int _batterySlots = 4;
    [SerializeField, ReadOnly] private int _currentBatterySlots;

    public int CurrentBatterySlots => _currentBatterySlots;
    public bool IsBatteryFull => _currentBatterySlots == _batterySlots;
    public bool IsBatteryEmpty => _currentBatterySlots == 0;

    [SerializeField] private List<SpriteRenderer> _batterySprites;
    [SerializeField] private Color _chargedBatterySlotColor = Color.blue;
    [SerializeField] private Color _unchargedBatterySlotColor = Color.gray;

    public UnityEvent OnBatteryDeath;
    public UnityEvent OnBatteryRecharged;
    public UnityEvent OnBatteryConsumed;

    private void Start()
    {
        _currentBatterySlots = _batterySlots;

        if (_batterySprites.Count != _batterySlots)
        {
            Debug.LogError("Battery sprites count does not match battery slots count.");
        }

        // Init tous les slots en "chargés"
        for (int i = 0; i < _batterySprites.Count; i++)
        {
            _batterySprites[i].color = _chargedBatterySlotColor;
        }
    }

    [Button]
    public void ConsumeBatterySlot()
    {
        if (_currentBatterySlots > 0)
        {
            Mathf.Clamp(_currentBatterySlots--, 0, _batterySlots);

            // Éteindre le slot correspondant
            _batterySprites[_currentBatterySlots].color = _unchargedBatterySlotColor;
            OnBatteryConsumed?.Invoke();
        }
        // Vérifie si plus de batterie
        if (_currentBatterySlots == 0)
        {
            OnBatteryDeath?.Invoke();
            Debug.Log("Battery empty - Player dies");
        }
    }
    
    [Button]
    public void ConsumeAllBattery()
    {
        _currentBatterySlots = 0;
        for (int i = 0; i < _batterySprites.Count; i++)
        {
            _batterySprites[i].color = _unchargedBatterySlotColor;
        }
        OnBatteryConsumed?.Invoke();
    }

    [Button]
    public void RechargeBatterySlot()
    {
        if (_currentBatterySlots < _batterySlots)
        {
            _batterySprites[_currentBatterySlots].color = _chargedBatterySlotColor;

            Mathf.Clamp(_currentBatterySlots++, 0, _batterySlots);
        }
        OnBatteryRecharged?.Invoke();
    }
    
    [Button]
    public void RechargeAllBattery()
    {
        while (!IsBatteryFull)
        {
            RechargeBatterySlot();
        }
        OnBatteryRecharged?.Invoke();
    }
}