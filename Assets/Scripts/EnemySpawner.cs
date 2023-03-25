using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemiesSO _enemiesContainer;
    [SerializeField] private int _maxSpawns = 10;
    [SerializeField] private int _maxSpawnedAmount = 3;
    [SerializeField] private float _timeBetweenSpawns = 1.0f;
    
    [SerializeField] private UnityEvent onSpawnEvent;
    [SerializeField] public UnityEvent onAllKilledEvent;

    [SerializeField] private List<GameObject> _spawnedEnemies;

    private float _timer = 0.0f;
    private int _maxSpawnCounter = 0;

    private void Update()
    {
        SpawnTick();
    }

    private void SpawnTick()
    {
        if (_maxSpawnCounter == _maxSpawns)
            return;

        _timer += Time.deltaTime;

        if (_timer < _timeBetweenSpawns) 
            return;
        
        _timer = 0.0f;
        Spawn();
    }
    
    public void SendDeath(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy.gameObject);

        if (AllSpawnablesKilled())
        {
            onAllKilledEvent?.Invoke();
        }
    }

    private void Spawn()
    {
        if (CanSpawn() == false)
            return;
        
        GameObject spawnedEnemy = Instantiate(_enemiesContainer.GetRandom(), null, true);
        spawnedEnemy.transform.position = transform.position;
        
        _spawnedEnemies.Add(spawnedEnemy);
        spawnedEnemy.GetComponent<Enemy>().SetSpawner(this);

        onSpawnEvent?.Invoke();
        
        SoundSystem.Instance.PlaySound("fx-spawner", transform);
        
        _maxSpawnCounter++;
    }

    private bool CanSpawn()
    {
        return _spawnedEnemies.Count < _maxSpawnedAmount;
    }

    public bool AllSpawnablesKilled()
    {
        if (_maxSpawnCounter < _maxSpawns) 
            return false;
        
        // Checks if enemies have been killed
        return _spawnedEnemies.Count == 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1.0f);
    }
#endif // UNITY_EDITOR
}
