using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;


public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Ground Check")] 
    [SerializeField, ReadOnly] private bool _isGrounded;
    public bool IsGrounded => _isGrounded;

    [SerializeField] private Vector2 _boxSize = new Vector2(0.5f, 0.2f);
    [SerializeField] private float _boxOffset = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Transform _pivot;        // centre de la planète
    private Rigidbody2D _rb;
    
    public UnityEvent OnPlayerJump;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f; // on ne veut pas de gravité Unity classique

        _pivot = transform.parent; 
        if (_pivot == null)
            Debug.LogWarning("[PlayerJump] Le Player doit être enfant d’un pivot centré sur la planète.");
    }

    private void Update()
    {
        GroundCheck();

        if (_isGrounded && Input.GetKeyDown(_jumpKey))
        {
            Jump();
        }
        
        Vector3 outwardDir = (transform.position - _pivot.position).normalized;
        transform.up = outwardDir;
    }

    private void Jump()
    {
        OnPlayerJump?.Invoke();
        Vector3 outward = (transform.position - _pivot.position).normalized;
        _rb.linearVelocity = Vector2.zero; // reset pour un saut clean
        _rb.AddForce(outward * _jumpForce, ForceMode2D.Impulse);
        _isGrounded = false;
    }

    private void GroundCheck()
    {
        _isGrounded = Physics2D.BoxCast(
            transform.position,
            _boxSize,
            transform.eulerAngles.z,
            -transform.up,
            _boxOffset,
            _groundLayer
        );
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position - transform.up * _boxOffset, transform.rotation, Vector3.one);
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(Vector3.zero, _boxSize);
        Gizmos.matrix = oldMatrix;
    }
}
