using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 100f; 
    [SerializeField] private PlayerCollision _playerCollision; // référence au Player

    private void Update()
    {
        float direction = 0f;

        if (Input.GetKey(KeyCode.Q))   // gauche
            direction = 1f;
        if (Input.GetKey(KeyCode.D))   // droite
            direction = -1f;

        // Vérifie si on est bloqué dans cette direction
        if (direction != 0 && direction == _playerCollision.BlockDirection)
        {
            direction = 0; // ignore cette entrée
        }

        if (direction != 0)
        {
            transform.Rotate(Vector3.forward, direction * _rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}