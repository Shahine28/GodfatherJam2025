using NaughtyAttributes;
using UnityEngine;


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
        _curveCursor = Mathf.Clamp01(_curveCursor + _stepPerClick);
        float shaped = _earthRotationEvolutionCurve.Evaluate(_curveCursor);
        _rotationSpeed = Mathf.LerpUnclamped(_initialRotationSpeed, _maxRotationSpeed, shaped);
    }
    
    [Button]
    private void DecreaseRotationSpeed()
    {
        _curveCursor = Mathf.Clamp01(_curveCursor - _stepPerClick);
        float shaped = _earthRotationEvolutionCurve.Evaluate(_curveCursor);
        _rotationSpeed = Mathf.LerpUnclamped(_initialRotationSpeed, _maxRotationSpeed, shaped);
    }
}