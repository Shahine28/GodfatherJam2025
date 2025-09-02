using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 100f; // degrés par seconde

    private void Update()
    {
        float direction = 0f;

        if (Input.GetKey(KeyCode.Q))   // gauche
            direction = 1f;
        if (Input.GetKey(KeyCode.D))   // droite
            direction = -1f;

        if (direction != 0f)
        {
            transform.Rotate(Vector3.forward, direction * _rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}