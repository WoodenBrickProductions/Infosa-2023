using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Camera _menuCamera;
    
    private void OnEnable()
    {
        _menuCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameState.Instance.RequestState(GameState.Type.Menu);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _menuCamera.gameObject.SetActive(false);
            RunManager.Instance.RestartRun();
        }
    }
}
