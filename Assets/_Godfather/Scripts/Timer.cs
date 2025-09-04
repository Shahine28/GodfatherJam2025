using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI _timerText;
    private float _timeElapsed;
    

    private void Awake()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;
        // Affiche le temps au format MM:SS:MS
        _timerText.text = GetTimeString();
    }
    
    public string GetTimeString()
    {
        int minutes = Mathf.FloorToInt(_timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(_timeElapsed % 60f);
        int milliseconds = Mathf.FloorToInt((_timeElapsed * 100f) % 100f);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
