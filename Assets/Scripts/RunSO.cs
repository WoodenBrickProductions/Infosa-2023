using UnityEngine;

[CreateAssetMenu(fileName = "RunSO", menuName = "RunSO", order = 0)]
public class RunSO : ScriptableObject
{
    [SerializeField] private int _runLength = 0; // if the length is set to 0, the run will be endless

    public int RunLength
    {
        get => _runLength;
    }

    public bool IsEndless
    {
        get => _runLength == 0;
    }
}