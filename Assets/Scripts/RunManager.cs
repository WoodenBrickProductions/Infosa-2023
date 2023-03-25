using System;
using Levels;
using UnityEngine;
using UnityEngine.Events;

public class RunManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private LevelsSO _levels;
    [SerializeField] private UnityEvent _onRunStart;
    [SerializeField] private UnityEvent _onNextRoom;
    [SerializeField] private UnityEvent _onRunEnd;

    private GameObject _player;
    private LevelBase _currentLevel;
    private RunSO _runData;

    private int _roomCounter = 0;

    private static RunManager _instance;

    public static RunManager Instance
    {
        get => _instance;
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartRun(RunSO runData)
    {
        _runData = runData;
        _playerObject = Instantiate(_playerPrefab, null);
        
        // TODO: add player interactable
        _roomCounter = 0;
        
        _currentLevel = Instantiate(_levels.GetRandom()).GetComponent<LevelBase>();
        _currentLevel.Initialize(this);
        
        SoundSystem.Instance.PlaySound("track-action");
        SoundSystem.Instance.PlaySound("fx-start-action");
        SoundSystem.Instance.StopSound("track-chill");
        
        _onRunStart?.Invoke();
    }

    public void NextRoom()
    {
        if (_runData.IsEndless == false && _runData.RunLength <= _roomCounter)
        {
            EndRun();
            return;
        }
        
        LevelBase lastLevel = _currentLevel;
        _currentLevel = Instantiate(_levels.GetRandom()).GetComponent<LevelBase>();
        _currentLevel.Initialize(this);
        Destroy(lastLevel.gameObject);

        SoundSystem.Instance.PlaySound("track-action");
        SoundSystem.Instance.PlaySound("fx-start-action");
        SoundSystem.Instance.StopSound("track-chill");
        
        _roomCounter++;
        
        // TODO: reset player

        _onNextRoom.Invoke();
    }

    // TODO: also run this if going back to main menu
    public void EndRun()
    {
        // TODO: destroy player
        Destroy(_playerObject);
        Destroy(_currentLevel.gameObject);
        
        SoundSystem.Instance.PlaySound("track-chill");
        SoundSystem.Instance.PlaySound("fx-stop-action");
        SoundSystem.Instance.StopSound("track-action");
        
        _onRunEnd.Invoke();
    }
}
