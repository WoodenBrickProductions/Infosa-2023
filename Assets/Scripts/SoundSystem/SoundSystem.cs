using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public SoundBank SoundBank;
    
    public List<SoundPlayer> players = new List<SoundPlayer>();

    private static SoundSystem _instance;

    public static SoundSystem Instance
    {
        get => _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var go = new GameObject();
            go.transform.parent = this.transform;
            _instance.players.Add(go.AddComponent<SoundPlayer>());
        }
    }

    void LateUpdate()
    {
        AudioTick();
    }

    void AudioTick()
    {
        
        
    }

    public void PlaySound(string sound)
    {
        // TODO! EFFICIENT
        if (SoundBank.data.Exists(item => item.name == sound))
        {
            var data = SoundBank.data.Find(item => item.name == sound);
            GetAvailablePlayer()?.Play(data);
        }
    }

    public void StopSound(string sound)
    {
        var player = players.Where(player => player.data.name == sound).FirstOrDefault();

        if (player != null)
        {
            player.Stop();
        }
    }

    private SoundPlayer GetAvailablePlayer()
    {
        // TODO! EFFICIENT
        return _instance.players.Where(player => player.source.isPlaying == false).FirstOrDefault();
    }

    [MenuItem("SoundSystem/Initialize in project")]
    public static void InitializeInProject()
    {
        // TODO! 
        
        Debug.Log("Initializing sound system in project");
        // Create directories
        
        
        // Create a mixer at the root of sounds directory
        
        // Create a first soundbank
        
        // Assign those assets to required fields
        
        // Refresh asset database
    }
}
