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

    public void Play(SoundData data, Transform playPosition = null)
    {
        this.data = data;
        isPlaying = true;
        source.clip = this.data.GetClip();
        source.volume = this.data.volume;

        if (playPosition != null)
        {
            transform.SetParent(null);
            transform.position = playPosition.position;
        }
        else
        {
            transform.SetParent(SoundSystem.Instance.transform);
        }
        
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
        
        transform.SetParent(SoundSystem.Instance.transform);
    }
}
