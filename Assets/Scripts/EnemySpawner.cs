using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemiesSO _enemiesContainer;

    private float _timer = 0.0f;

    private void Update()
    {
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
    }
}
