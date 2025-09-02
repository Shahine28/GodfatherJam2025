using NaughtyAttributes;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump & Gravity")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpSpeed = 5f;          // vitesse de départ (unités/s)
    [SerializeField] private float _gravity = 20f;           // accélération vers le centre (unités/s²)
    [SerializeField] private float _groundSnap = 0.02f;      // marge pour recoller au sol

    [Header("Orientation")]
    [SerializeField] private bool _alignUpWithOutward = true; // aligne l'Up du joueur vers l'extérieur

    [Header("Debug")]
    [SerializeField, ReadOnly] private bool _isGrounded;
    [SerializeField, ReadOnly] private float _surfaceRadius;   // rayon du sol (distance centre → surface)
    [SerializeField, ReadOnly] private float _currentRadius;   // rayon actuel
    [SerializeField, ReadOnly] private float _radialVelocity;  // + = sort du centre ; - = tombe

    private Transform _pivot; // normalement PlayerRotator (centre de la planète)

    private void Awake()
    {
        _pivot = transform.parent; // pivot = centre
        if (_pivot == null)
            Debug.LogWarning("[PlanetJump2D] Le Player doit être enfant d’un pivot centré sur la planète.");
    }

    private void Start()
    {
        // On prend la distance initiale comme rayon de surface
        _surfaceRadius = transform.localPosition.magnitude;
        _currentRadius = _surfaceRadius;
        _isGrounded = true;
    }

    private void Update()
    {
        // Input saut
        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            _isGrounded = false;
            _radialVelocity = _jumpSpeed; // impulsion vers l’extérieur
        }

        SimulateRadialMotion();

        if (_alignUpWithOutward && _pivot != null)
        {
            Vector3 outward = (transform.position - _pivot.position).normalized;
            transform.up = outward; // le joueur "regarde" vers l’extérieur
        }
    }

    private void SimulateRadialMotion()
    {
        // Direction radiale locale (ne change pas avec la rotation du pivot)
        Vector3 localDir = transform.localPosition.sqrMagnitude > 0.000001f
            ? transform.localPosition.normalized
            : Vector3.up; // fallback

        if (!_isGrounded)
        {
            _radialVelocity -= _gravity * Time.deltaTime;               // gravité vers le centre
            _currentRadius  += _radialVelocity * Time.deltaTime;        // intégration simple

            // Contact avec la surface : on coupe la gravité et on recolle
            if (_currentRadius <= _surfaceRadius + _groundSnap && _radialVelocity <= 0f)
            {
                _currentRadius  = _surfaceRadius;
                _radialVelocity = 0f;
                _isGrounded     = true;
            }
        }

        // Applique la nouvelle position en conservant l’angle d’orbite
        transform.localPosition = localDir * _currentRadius;
    }
}
