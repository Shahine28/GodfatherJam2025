using UnityEngine;

public class AudioManager : MonoBehaviour
{
private AudioSource source;

private void Start()
{
source = GetComponent<AudioSource>();
}

public void PlayClip(AudioClip clip)
{
source.clip = clip;
source.Play();
}

}
