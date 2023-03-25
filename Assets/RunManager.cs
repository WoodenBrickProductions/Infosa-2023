using System;
using Levels;
using UnityEngine;
using UnityEngine.Events;

public class RunManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;

    [SerializeField] private RunSO _runData;
    [SerializeField] private LevelsSO _levels;
    [SerializeField] private UnityEvent _onRunStart;
    [SerializeField] private UnityEvent _onNextRoom;
    [SerializeField] private UnityEvent _onRunEnd;

    private GameObject _player;
    private LevelBase _currentLevel;

    private int _roomCounter = 0;

    // TODO: remove later
    private void Start()
    {
        StartRun();
    }

    public void StartRun()
    {
        // TODO: add player interactable
        _roomCounter = 0;
        
        _currentLevel = Instantiate(_levels.GetRandom()).GetComponent<LevelBase>();
        _currentLevel.Initialize(this);
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

        _roomCounter++;
        
        // TODO: reset player

        _onNextRoom.Invoke();
    }

    // TODO: also run this if going back to main menu
    public void EndRun()
    {
        // TODO: destroy player
        
        Destroy(_currentLevel.gameObject);
        
        _onRunEnd.Invoke();
    }
}
