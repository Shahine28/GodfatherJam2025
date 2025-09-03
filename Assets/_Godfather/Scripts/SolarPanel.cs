using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    [SerializeField] private LayerMask _layerToSnap;
    void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, Mathf.Infinity, _layerToSnap);
        if (hit)
        {
            if (hit.collider != null)
            {
                transform.position = hit.point;
            }
        }
    }
}
