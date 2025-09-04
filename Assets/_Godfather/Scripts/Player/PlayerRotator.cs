using UnityEngine;
using UnityEngine.InputSystem; // obligatoire pour CallbackContext

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private EarthBehavior _earthBehavior;

    private float _direction; // -1 droite, 1 gauche, 0 neutre

    // === Appelé par l'Input System (Invoke Unity Events) ===
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();

        if (inputValue.x < 0) 
            _direction = 1f;
        else if (inputValue.x > 0)
            _direction = -1f;
        else
            _direction = 0f;
    }

    private void Update()
    {
        float direction = _direction;


        if (direction > 0f)
            _playerSpriteRenderer.flipX = true;   // gauche
        else if (direction < 0f)
            _playerSpriteRenderer.flipX = false;  // droite


        if (direction != 0 && direction == _playerCollision.BlockDirection)
        {
            direction = 0;
        }


        if (direction != 0)
        {
            float finalRotationSpeed = _rotationSpeed;
            if (_rotationSpeed < _earthBehavior.RotationSpeed)
            {
                finalRotationSpeed = _rotationSpeed + _earthBehavior.RotationSpeed;
            }

            _playerAnimator.SetBool("walk", true);
            transform.Rotate(Vector3.forward, direction * finalRotationSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            _playerAnimator.SetBool("walk", false);
        }
    }
}
