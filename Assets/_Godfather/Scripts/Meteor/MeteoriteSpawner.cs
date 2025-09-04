using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NaughtyAttributes;
using UnityEngine.Events;

public class MeteoriteSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _meteoritePrefab;
    [SerializeField] private GameObject _warningPrefab; // un UI avec Image "!"

    [Header("Spawn Settings")]
    [SerializeField] private float _warningTime = 1.5f;
    [SerializeField] private float _spawnOffset = 2f; // distance derrière le bord de l’écran

    [SerializeField] private Canvas _canvas; 

    private Camera _cam;
    
    [Header("Sun Avoidance")]
    [SerializeField] private LayerMask _sunLayer;
    [SerializeField] private float _checkRadius = 1f; // rayon de vérif pour éviter le soleil
    [SerializeField] private int _maxAttempts = 10;   // limite pour éviter boucle infinie

    
    [Header("Meteorite Spawning")]
    [SerializeField] private float _spawnInterval = 5f;
    private float _spawnTimer;

[SerializeField] Transform _earthCenter;

    public UnityEvent OnMeteoriteWarning;
    public UnityEvent OnMeteoriteSpawned;
    private void Start()
    {
        _cam = Camera.main;
    }
    
    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnMeteorite();
        }
    }
    
    [Button]
    public void SpawnMeteoriteRandom()
    {
        SpawnMeteorite();
    }
    
    public void SpawnMeteorite()
    {
        Vector3 spawnPos = Vector3.zero;
        Vector3 uiPos = Vector3.zero;
        bool valid = false;

        int attempts = 0;

        while (!valid && attempts < _maxAttempts)
        {
            attempts++;

            // Choisir un côté au hasard
            int side = Random.Range(0, 4); // 0=Gauche,1=Droite,2=Haut,3=Bas

            // Position en viewport (0..1)
            Vector3 viewportPos = Vector3.zero;
            switch (side)
            {
                case 0: viewportPos = new Vector3(0f, Random.value, _cam.nearClipPlane); break; // gauche
                case 1: viewportPos = new Vector3(1f, Random.value, _cam.nearClipPlane); break; // droite
                case 2: viewportPos = new Vector3(Random.value, 1f, _cam.nearClipPlane); break; // haut
                case 3: viewportPos = new Vector3(Random.value, 0f, _cam.nearClipPlane); break; // bas
            }

            // Convertir en monde (bord exact de l’écran)
            Vector3 worldEdge = _cam.ViewportToWorldPoint(viewportPos);

            // Direction vers le centre
            Vector3 toCenter = (_cam.transform.position - worldEdge).normalized;

            // Position de spawn hors écran
            spawnPos = worldEdge + toCenter * -_spawnOffset;

            // Vérifie si la zone est libre (pas de Sun)
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, _checkRadius, _sunLayer);
            if (hit == null)
            {
                valid = true;

                // Convertir la pos viewport en UI pos (pixel)
                uiPos = new Vector3(
                    (viewportPos.x - 0.5f) * _canvas.pixelRect.width,
                    (viewportPos.y - 0.5f) * _canvas.pixelRect.height,
                    0f
                );
                
                
                GameObject warning = Instantiate(_warningPrefab, _canvas.transform);
                OnMeteoriteWarning?.Invoke();
                RectTransform rt = warning.GetComponent<RectTransform>();

                
                Vector2 size = rt.sizeDelta;

               
                Vector3 pos = uiPos;

                // Ajuste en fonction du bord choisi
                if (viewportPos.x <= 0f) // gauche
                    pos.x += size.x * 0.5f;
                else if (viewportPos.x >= 1f) // droite
                    pos.x -= size.x * 0.5f;

                if (viewportPos.y <= 0f) // bas
                    pos.y += size.y * 0.5f;
                else if (viewportPos.y >= 1f) // haut
                    pos.y -= size.y * 0.5f;

                rt.anchoredPosition = pos;
                
                // Lancer la coroutine
                StartCoroutine(SpawnWithWarning(spawnPos, warning));
            }
        }
        if (!valid)
        {
            Debug.LogWarning("Impossible de trouver un spawn valide pour la météorite.");
        }
    }

    private IEnumerator SpawnWithWarning(Vector3 spawnPos, GameObject warning)
    {
        yield return new WaitForSeconds(_warningTime);

        Destroy(warning);

        var dx = _earthCenter.position.x - spawnPos.x;
        var dy = _earthCenter.position.y - spawnPos.y;

        var angle = Mathf.Atan2(dy, dx);
        var angleDeg = Mathf.Rad2Deg * angle;
        var rot = Quaternion.Euler(0,0,angleDeg+90);

        var go = Instantiate(_meteoritePrefab, spawnPos, rot);

        OnMeteoriteSpawned?.Invoke();
    }
}
