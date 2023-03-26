using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _instance;
    [SerializeField] private Type _currentState = Type.Menu;

    public static GameState Instance
    {
        get => _instance;
    }

    public static Action<Type> OnStateChange;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum Type
    {
        Menu = 0,
        InGame = 1,
        Paused = 2,
        GameOver = 3,
    }

    public Type GetState()
    {
        return _currentState;
    }

    public void RequestState(Type type)
    {
        // TODO! add proper game state request handling 
        
        _currentState = type;
        
        OnStateChange?.Invoke(_currentState);
        
        Debug.Log(_currentState);
    }
}
