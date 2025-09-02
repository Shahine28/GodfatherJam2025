using NaughtyAttributes;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump & Gravity")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpForce = 5f;   // impulsion initiale
    [SerializeField] private float _gravity = 20f;    // force vers le pivot

    [Header("Ground Check")] 
    [SerializeField, ReadOnly] private bool _isGrounded;
    [SerializeField] private Vector2 _boxSize = new Vector2(0.5f, 0.2f);
    [SerializeField] private float _boxOffset = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Transform _pivot; // centre de la planète
    private Vector3 _velocity; // vitesse actuelle (radiale)

    private void Awake()
    {
        _pivot = transform.parent; 
        if (_pivot == null)
            Debug.LogWarning("[PlayerJump] Le Player doit être enfant d’un pivot centré sur la planète.");
    }

    private void Update()
    {
        GroundCheck();

        if (_isGrounded)
        {
            _velocity = Vector3.zero; // reset vitesse

            if (Input.GetKeyDown(_jumpKey))
            {
                // impulsion vers l’extérieur (depuis pivot)
                Vector3 outward = (transform.position - _pivot.position).normalized;
                _velocity = outward * _jumpForce;
                _isGrounded = false;
            }
        }
        else
        {
            // appliquer gravité vers le pivot
            Vector3 toCenter = (_pivot.position - transform.position).normalized;
            _velocity += toCenter * _gravity * Time.deltaTime;
        }

        // applique le déplacement
        transform.position += _velocity * Time.deltaTime;

        // --- FORCER L'ORIENTATION : pieds vers la planète ---
        Vector3 outwardDir = (transform.position - _pivot.position).normalized;
        transform.up = outwardDir; // "tête vers l'extérieur", "pieds vers le centre"
    }

    private void GroundCheck()
    {
        _isGrounded = Physics2D.BoxCast(transform.position, _boxSize, transform.eulerAngles.z, -transform.up, _boxOffset, _groundLayer);
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
