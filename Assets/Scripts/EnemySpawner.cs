using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemiesSO _enemiesContainer;
    [SerializeField] private int _spawnAmount = 1;
    [SerializeField] private UnityEvent onSpawnEvent;   

    private float _timer = 0.0f;
    private int _spawnCounter = 0;

    private void Update()
    {
        if (_spawnCounter == _spawnAmount)
            return;
        
        _timer += Time.deltaTime;

        if (_timer >= 5.0f)
        {
            _timer = 0.0f;

            Spawn();
        }
    }

    public void Spawn()
    {
        var spawnedEnemy = Instantiate(_enemiesContainer.GetRandom());

        spawnedEnemy.transform.SetParent(null);
        spawnedEnemy.transform.position = transform.position;

        _spawnCounter++;
        
        onSpawnEvent?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(transform.position, 1.0f);
    }
#endif // UNITY_EDITOR
}
