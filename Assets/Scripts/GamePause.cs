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
        } 
        else if (GameState.Instance.GetState().Equals(GameState.Type.Paused))
        {
            GameState.Instance.RequestState(GameState.Type.InGame);
        }
    }
}
