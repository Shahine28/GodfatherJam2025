using UnityEngine;
using UnityEngine.Events;

public class MeteorBehavior : MonoBehaviour
{
    public UnityEvent OnMeteorDestroyed;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            other.gameObject.GetComponent<EarthBehavior>()?.IncreaseRotationSpeed();
        }
        else if (other.gameObject.CompareTag("SolarPanel"))
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerBattery>()?.ConsumeBatterySlot();
        }
        OnMeteorDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
