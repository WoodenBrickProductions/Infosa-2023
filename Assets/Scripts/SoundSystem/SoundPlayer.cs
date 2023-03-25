using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public SoundData data;
    public AudioSource source;
    public bool isPlaying;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.Stop();
        isPlaying = false;
    }
    

    public void Play(SoundData data)
    {
        this.data = data;
        isPlaying = true;
        source.clip = this.data.GetClip();

        if (data.type == SoundData.Type.MusicLayer)
        {
            source.loop = true;
        }
        else
        {
            source.loop = false;
        }
        
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
