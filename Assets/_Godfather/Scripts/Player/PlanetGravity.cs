using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    [SerializeField] protected GameObject _planet;
    private Rigidbody2D _rb;
    [SerializeField] private float _gravityForce = 9.81f;
    [SerializeField] private float _gravityDistance = 5f;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_planet == null)
        {
            _planet = GameObject.FindWithTag("Planet");
        }
    }

    // Update is called once per frame
    void Update()
    {
        SimulateGravity();
    }
    
    protected virtual void SimulateGravity()
    {
        float distance = Vector3.Distance(transform.position, _planet.transform.position);
        Vector3 vector = _planet.transform.position - transform.position;
        _rb.AddForce(vector.normalized * (_gravityForce / (distance / _gravityDistance)));
    }
}
