using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> _spawners;
    [SerializeField] private Interactable _levelExitInteractable;
    [SerializeField] private RunManager _runManager;

    public void Initialize(RunManager runManager)
    {
        _runManager = runManager;
        _levelExitInteractable.OnInteraction.AddListener(_runManager.NextRoom);

        foreach (EnemySpawner spawner in _spawners)
        {
            spawner.onAllKilledEvent.AddListener(TryEnableExitInteractable);
        }
    }

    private void TryEnableExitInteractable()
    {
        foreach (var spawner in _spawners)
        {
            if (spawner.AllSpawnablesKilled() == false)
                return;
        }
        
        SoundSystem.Instance.PlaySound("track-chill");
        SoundSystem.Instance.PlaySound("fx-stop-action");
        SoundSystem.Instance.StopSound("track-action");
        
        _levelExitInteractable.EnableInteraction();
    }

    public void OnDisable()
    {
        _levelExitInteractable.OnInteraction.RemoveListener(_runManager.NextRoom);
        foreach (EnemySpawner spawner in _spawners)
        {
            spawner.onAllKilledEvent.RemoveListener(TryEnableExitInteractable);
        }
    }
}
