using UnityEngine;
public class MeteorBehavior : MonoBehaviour
{
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
        Destroy(gameObject);
    }
}
