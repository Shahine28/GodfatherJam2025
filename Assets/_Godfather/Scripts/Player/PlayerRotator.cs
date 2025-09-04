using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 100f; 
    [SerializeField] private PlayerCollision _playerCollision; // référence au Player

    [SerializeField] private EarthBehavior _earthBehavior;
    
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
            direction = 0; 
        }

        if (direction != 0)
        {
            float _finalRotationSpeed = _rotationSpeed;
            if (_rotationSpeed < _earthBehavior.RotationSpeed)
            {
                _finalRotationSpeed = _rotationSpeed + _earthBehavior.RotationSpeed;
            }
            transform.Rotate(Vector3.forward, direction * _finalRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}