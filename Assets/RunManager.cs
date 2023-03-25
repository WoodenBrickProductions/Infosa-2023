using System;
using Levels;
using UnityEngine;
using UnityEngine.Events;

public class RunManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    
    [SerializeField] private LevelsSO _levels;
    [SerializeField] private UnityEvent _onRunStart;
    [SerializeField] private UnityEvent _onNextRoom;
    [SerializeField] private UnityEvent _onRunEnd;

    private GameObject _player;
    private LevelBase _currentLevel;

    // TODO: remove later
    private void Start()
    {
        StartRun();
    }

    public void StartRun()
    {
        // TODO: add player interactable
        
        _currentLevel = Instantiate(_levels.GetRandom()).GetComponent<LevelBase>();
        _currentLevel.Initialize(this);
        _onRunStart?.Invoke();
    }

    public void NextRoom()
    {
        LevelBase lastLevel = _currentLevel;
        _currentLevel = Instantiate(_levels.GetRandom()).GetComponent<LevelBase>();
        _currentLevel.Initialize(this);
        Destroy(lastLevel.gameObject);
        
        // TODO: reset player

        _onNextRoom.Invoke();
    }

    public void EndRun()
    {
        Destroy(_player);
        
        _onRunEnd.Invoke();
    }
}
