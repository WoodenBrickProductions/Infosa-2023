using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public SoundData data;
    public AudioSource source;
    public bool isPlaying;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>(); 
    }

    public void Play(SoundData data)
    {
        this.data = data;
        isPlaying = true;
        source.clip = this.data.clip;
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
        data = null;
        source.clip = null;
        isPlaying = false;
    }
}
