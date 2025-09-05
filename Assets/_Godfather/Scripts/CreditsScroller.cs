using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _creditsContent; 
    [SerializeField] private float _scrollSpeed = 50f;      
    [SerializeField] private Vector3 _startPosition;        
    [SerializeField] private Vector3 _endPosition;          

    private bool _isScrolling;

    private void Start()
    {
        // Assure que le contenu commence à la position de départ
        _creditsContent.anchoredPosition = _startPosition;
    }
    
    public void StartScroll()
    {
        gameObject.SetActive(true);
        _creditsContent.anchoredPosition = _startPosition;
        _isScrolling = true;
    }

    private void Update()
    {
        if (!_isScrolling) return;

        // Avance vers la position de fin
        _creditsContent.anchoredPosition = Vector3.MoveTowards(
            _creditsContent.anchoredPosition,
            _endPosition,
            _scrollSpeed * Time.deltaTime
        );

        // Arrivé à la fin → reset et stop
        if (Vector3.Distance(_creditsContent.anchoredPosition, _endPosition) <= 0.01f)
        {
            _creditsContent.anchoredPosition = _startPosition;
            _isScrolling = false;
            gameObject.SetActive(false);
        }
    }
}