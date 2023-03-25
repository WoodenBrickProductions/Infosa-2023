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

    public void StartRun()
    {
        _player = Instantiate(_playerObject);
        
        

        _onRunStart?.Invoke();
    }

    public void NextRoom()
    {
        // TODO: instantiate a room, remove last room, set player params
        
        _onNextRoom.Invoke();
    }

    public void EndRun()
    {
        Destroy(_player);
        
        _onRunEnd.Invoke();
    }
}
