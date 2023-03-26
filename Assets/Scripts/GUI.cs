using System;
using UnityEngine;

public class GUI : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;

    private void Start()
    {
        OnGameStateChange(GameState.Type.Menu);
    }

    private void OnEnable()
    {
        GameState.OnStateChange += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameState.OnStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState.Type obj)
    {
        _pauseMenu.SetActive(obj.Equals(GameState.Type.Paused));
        _menu.SetActive(obj.Equals(GameState.Type.Menu));
        _gameOverMenu.SetActive(obj.Equals(GameState.Type.GameOver));
    }
}
