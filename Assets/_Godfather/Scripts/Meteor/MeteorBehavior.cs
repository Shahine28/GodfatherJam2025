using UnityEngine;
using UnityEngine.Events;

public class MeteorBehavior : MonoBehaviour
{
    [SerializeField] SpriteRenderer _img;
    [SerializeField] Collider2D _collider2D;

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


        OnMeteorDestroyed?.Invoke();
        _img.enabled=false;
        _collider2D.enabled=false;
        Destroy(gameObject, 1f);
    }
}
