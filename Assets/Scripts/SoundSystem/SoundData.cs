using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string name;
    public AudioClip clip;
    public Type type;

    public enum Type
    {
        MusicLayer = 0,
        SfxOneshot = 10
    }
}