using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayVFX()
    {
        _vfx.gameObject.SetActive(true);
        _vfx.Play();
    }
    
    public void StopVFX()
    {
        _vfx.Stop();
        _vfx.gameObject.SetActive(false);
    }
}
