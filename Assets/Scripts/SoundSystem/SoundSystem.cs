using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public SoundBank SoundBank;
    
    public List<SoundPlayer> players = new List<SoundPlayer>();

    private static SoundSystem _instance;
    private bool _initialized = false;

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
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < 20; i++)
        {
            var go = new GameObject();
            go.transform.parent = transform;
            _instance.players.Add(go.AddComponent<SoundPlayer>());
        }

        _initialized = true;
    }

    void LateUpdate()
    {
        AudioTick();
    }

    void AudioTick()
    {
        
        
    }

    public void PlaySound(string sound, Transform soundPosition = null)
    {
        // failsafe
        if (_initialized == false)
        {
            Initialize();
        }
        
        // TODO! EFFICIENT
        if (SoundBank.data.Exists(item => item.name == sound))
        {
            var data = SoundBank.data.Find(item => item.name == sound);

            if (soundPosition == null)
            {
                GetAvailablePlayer()?.Play(data);
            }
            else
            {
                GetAvailablePlayer()?.Play(data, soundPosition);
            }
        }
    }

    public void StopSound(string sound)
    {
        if (players.Count == 0)
            return;

        var player = players.Where(player => player.data != null && player.data.name == sound).FirstOrDefault();

        if (player != null)
        {
            Initialize();
        }
        
        player?.Stop();
    }

    private SoundPlayer GetAvailablePlayer()
    {
        // TODO! EFFICIENT
        return _instance.players.FirstOrDefault(player => player.source.isPlaying == false);
    }
}
