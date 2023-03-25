using UnityEngine;

public class GUI : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _inGameMenu;
    [SerializeField] private GameObject _pauseMenu;

    private void Awake()
    {
        Refresh(GameState.Instance.GetState());
        GameState.OnStateChange += Refresh;
    }

    private void OnDestroy()
    {
        GameState.OnStateChange -= Refresh;
    }

    private void Refresh(GameState.Type type)
    {
        switch (type)
        {
            case GameState.Type.Menu:
                EnableScreen(_mainMenu);
                break;
            case GameState.Type.InGame:
                EnableScreen(_inGameMenu);
                break;
            case GameState.Type.Paused:
                EnableScreen(_pauseMenu);
                break;
        }
    }

    private void EnableScreen(GameObject screenToEnable)
    {
        _mainMenu.SetActive(_mainMenu == screenToEnable);
        _pauseMenu.SetActive(_pauseMenu == screenToEnable);
        _inGameMenu.SetActive(_inGameMenu == screenToEnable);
    }
}
