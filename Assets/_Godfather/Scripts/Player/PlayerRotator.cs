using UnityEngine;
using UnityEngine.InputSystem; // obligatoire pour CallbackContext

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private EarthBehavior _earthBehavior;
    
    
    [SerializeField] private PlayerJump _playerJump;

    private float _direction; 


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
        if (_playerJump != null)
        {
            if (!_playerJump.IsGrounded && transform.parent == _earthBehavior.transform)
            {
                // détache du parent (plus de rotation héritée)
                transform.SetParent(null, true); 
            }
            else if (_playerJump.IsGrounded && transform.parent == null)
            {
                // rattache à la Terre
                transform.SetParent(_earthBehavior.transform, true);
            }
        }

        if (!_playerJump.IsGrounded) return;
        
        float direction = _direction;

        if (direction > 0f)
            _playerSpriteRenderer.flipX = true;
        else if (direction < 0f)
            _playerSpriteRenderer.flipX = false;

        if (direction != 0 && direction == _playerCollision.BlockDirection)
        {
            direction = 0;
        }

        if (direction != 0)
        {
            float finalRotationSpeed = _rotationSpeed;
            _playerAnimator.SetBool("walk", true);
            transform.Rotate(Vector3.forward, direction * finalRotationSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            _playerAnimator.SetBool("walk", false);
        }
    }
}
