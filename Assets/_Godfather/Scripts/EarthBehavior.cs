using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;


public class EarthBehavior : MonoBehaviour
{
    [Header("Speed Range")]
    [SerializeField] private float _initialRotationSpeed = 10f;
    [SerializeField] private float _maxRotationSpeed = 1000f;
    [SerializeField, ReadOnly] private float _rotationSpeed;
    public float RotationSpeed => _rotationSpeed;

    [Header("Rotation Speed Control")]
    [SerializeField, Range(0.01f, 1f)] private float _stepPerClick = 0.05f; // avance du curseur de la courbe par clic
    [SerializeField]  private AnimationCurve _earthRotationEvolutionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float _curveCursor; // curseur 0..1
    
    [Header("Camera Flip")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _cameraFlipDuration = 1f;
    public UnityEvent OnCameraFlip;
    
    
    public UnityEvent OnEarthSpeedIncreased;
    public UnityEvent OnEarthHitByMeteorite;

    private void Start()
    {
        _rotationSpeed = _initialRotationSpeed;
        _curveCursor = 0f; // 0 => vitesse = initiale
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }

    [Button]
    public void IncreaseRotationSpeed()
    {
        OnEarthSpeedIncreased?.Invoke();
        _curveCursor = Mathf.Clamp01(_curveCursor + _stepPerClick);
        float shaped = _earthRotationEvolutionCurve.Evaluate(_curveCursor);
        float newRotationSpeed = Mathf.LerpUnclamped(_initialRotationSpeed, _maxRotationSpeed, shaped);
        if (_rotationSpeed < _maxRotationSpeed/2 && newRotationSpeed >= _maxRotationSpeed/2)
        {
            FlipCamera();
        }
        _rotationSpeed = newRotationSpeed;
        
    }
    
    [Button]
    private void DecreaseRotationSpeed()
    {
        _curveCursor = Mathf.Clamp01(_curveCursor - _stepPerClick);
        float shaped = _earthRotationEvolutionCurve.Evaluate(_curveCursor);
        _rotationSpeed = Mathf.LerpUnclamped(_initialRotationSpeed, _maxRotationSpeed, shaped);
    }

    [Button]
    public void FlipCamera()
    {
        StopAllCoroutines();
        StartCoroutine(FlipCameraCoroutine());
    }
    
    IEnumerator FlipCameraCoroutine()
    {
        Quaternion initialRotation = _cameraTransform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, 180f);
        
        float elapsed = 0f;
        OnCameraFlip?.Invoke();
        while (elapsed < _cameraFlipDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _cameraFlipDuration);
            _cameraTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            yield return null;
        }
        _cameraTransform.rotation = targetRotation;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Meteor"))
        {
            OnEarthHitByMeteorite?.Invoke();
        }
    }
}