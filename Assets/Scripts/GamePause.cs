using UnityEngine;

public class GamePause : MonoBehaviour
{
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) 
            return;

        if (GameState.Instance.GetState().Equals(GameState.Type.InGame))
        {
            GameState.Instance.RequestState(GameState.Type.Paused);
            Time.timeScale = 0.0f;
        } 
        else if (GameState.Instance.GetState().Equals(GameState.Type.Paused))
        {
            GameState.Instance.RequestState(GameState.Type.InGame);
            Time.timeScale = 1.0f;
        }
    }
}
