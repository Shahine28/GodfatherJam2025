using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    [Header("Build Settings")]
    [SerializeField] private KeyCode _buildKey = KeyCode.E;
    [SerializeField] private GameObject _solarPanelPrefab;
    [SerializeField] private float _minDistanceBetweenPanels = 0.5f; // distance de sécurité
    [SerializeField] private LayerMask _panelLayer;
    // couche où sont les panneaux
    [SerializeField] private GameObject _earth; // référence à la planète

    private Transform _pivot; // centre de la planète

    private bool CanPlacePanel;
    private void Awake()
    {
        _pivot = transform.parent;
        if (_pivot == null)
            Debug.LogWarning("[PlayerBuild] Le Player doit être enfant d’un pivot centré sur la planète.");
        CanPlacePanel = true;
    }

    private void Update()
    {
        CheckPanel();
        if (Input.GetKeyDown(_buildKey))
        {
            TryPlacePanel();
        }
    }

    private void TryPlacePanel()
    {
        if (!CanPlacePanel) return;
        
        Vector3 outward = (transform.position - _pivot.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, outward);
        
        Instantiate(_solarPanelPrefab, transform.position, rotation, _earth.transform);
    }

    public void CheckPanel()
    {
        CanPlacePanel = !Physics2D.OverlapCircle(transform.position, _minDistanceBetweenPanels, _panelLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = CanPlacePanel ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, _minDistanceBetweenPanels);
    }
}