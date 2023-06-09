using Levels;
using UnityEngine;
using UnityEngine.Events;

public class RunManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private RunSO _endlessRunData;

    [SerializeField] private LevelsSO _levels;
    [SerializeField] private UnityEvent _onRunStart;
    [SerializeField] private UnityEvent _onNextRoom;
    [SerializeField] private UnityEvent _onRunEnd;

    public Player _player { private set; get; }
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

    public void StartRun()
    {
        StartRun(_endlessRunData);
    }

    public void StartRun(RunSO runData)
    {
        _runData = runData;
        _playerObject = Instantiate(_playerPrefab, null);
        _player = _playerObject.GetComponent<Player>();
        HUD.instance.UpdateHealth();

        // TODO: add player interactable
        _roomCounter = 0;
        
        _currentLevel = Instantiate(_levels.GetRandom()).GetComponent<LevelBase>();
        _currentLevel.Initialize(this);
        
        GameState.Instance.RequestState(GameState.Type.InGame);
        
        SoundSystem.Instance.StopSound("track-chill");
        SoundSystem.Instance.PlaySound("track-action");
        SoundSystem.Instance.PlaySound("fx-start-action");
        
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
        if(lastLevel.gameObject != null)
            Destroy(lastLevel.gameObject);
        
        SoundSystem.Instance.PlaySound("fx-start-action");

        SoundSystem.Instance.StopSound("track-chill");
        SoundSystem.Instance.PlaySound("track-action");
        
        _roomCounter++;
        
        // TODO: reset player

        _onNextRoom.Invoke();
    }

    // TODO: also run this if going back to main menu
    public void EndRun()
    {
        // TODO: destroy player
        if(_playerObject != null)
            Destroy(_playerObject);
        if(_currentLevel != null)
            Destroy(_currentLevel.gameObject);
        
        SoundSystem.Instance.PlaySound("fx-stop-action");
        
        SoundSystem.Instance.StopSound("track-chill");
        SoundSystem.Instance.StopSound("track-action");
        SoundSystem.Instance.PlaySound("track-chill");
        
        _onRunEnd.Invoke();
    }

    public void RestartRun()
    {
        EndRun();
        StartRun(_endlessRunData);
    }
}
