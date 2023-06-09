using UnityEngine;

public class RunLauncher : MonoBehaviour
{
    [SerializeField] private RunSO _runData;

    public void StartRun()
    {
        RunManager.Instance.StartRun(_runData);
    }

    public void StopRun()
    {
        RunManager.Instance.EndRun();
        GameState.Instance.RequestState(GameState.Type.Menu);
    }
}