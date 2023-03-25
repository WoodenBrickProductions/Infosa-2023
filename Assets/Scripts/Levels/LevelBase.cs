using System;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Interactable _levelExitInteractable;

    [SerializeField] private RunManager _runManager;

    public void Initialize(RunManager runManager)
    {
        _runManager = runManager;
        _levelExitInteractable.OnInteraction.AddListener(_runManager.NextRoom);
    }

    // TODO: remove this later
    public void Start()
    {
        _levelExitInteractable?.OnInteraction.AddListener(_runManager.NextRoom);
    }

    public void OnDestroy()
    {
        _levelExitInteractable.OnInteraction.RemoveListener(_runManager.NextRoom);
    }


    public void EnableExitInteractable()
    {
        
    }
}
