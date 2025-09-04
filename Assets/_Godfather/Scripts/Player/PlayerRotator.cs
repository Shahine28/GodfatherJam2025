using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 100f; 
    [SerializeField] private PlayerCollision _playerCollision; // référence au Player

    [SerializeField] private EarthBehavior _earthBehavior;
    [SerializeField] private Animator _playerAnimator;
    
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    private void Update()
    {
        float direction = 0f;

        if (Input.GetKey(KeyCode.Q))   // gauche
            direction = 1f;
        if (Input.GetKey(KeyCode.D))   // droite
            direction = -1f;

        // --- Flip sprite en fonction de la direction ---
        if (direction > 0f)
            _playerSpriteRenderer.flipX = true;   // vers la gauche
        else if (direction < 0f)
            _playerSpriteRenderer.flipX = false; 


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
            _playerAnimator.SetBool("walk", true);
            transform.Rotate(Vector3.forward, direction * _finalRotationSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            _playerAnimator.SetBool("walk", false);
        }
    }
}