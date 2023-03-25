using UnityEngine;

public class RunLauncher : MonoBehaviour
{
    [SerializeField] private RunSO _runData;

    public void StartRun()
    {
        RunManager.Instance.StartRun(_runData);
        GameState.Instance.RequestState(GameState.Type.InGame);
    }
}