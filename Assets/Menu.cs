using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private Camera _menuCamera;
    [SerializeField] private GameObject _menuLevel;

    public void Show()
    {
        _menuCamera.gameObject.SetActive(true);
        _menuLevel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _menuCamera.gameObject.SetActive(false);
        _menuLevel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameState.OnStateChange += OnGameStateChange;
        OnGameStateChange(GameState.Instance.GetState());
    }

    private void OnDisable()
    {
        GameState.OnStateChange -= OnGameStateChange;
    }
    
    private void OnGameStateChange(GameState.Type obj)
    {
        if (!obj.Equals(GameState.Type.Menu))
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Update()
    {
        if (_menuCamera.gameObject.activeSelf == false || _menuLevel.gameObject.activeSelf == false)
            return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<Quit>().StopApplication();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RunManager.Instance.StartRun();
        }
    }
}
