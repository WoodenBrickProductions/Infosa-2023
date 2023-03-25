using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string name;
    public AudioClip[] clips;
    public Type type;

    public enum Type
    {
        MusicLayer = 0,
        SfxOneshot = 10
    }

    public AudioClip GetClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}