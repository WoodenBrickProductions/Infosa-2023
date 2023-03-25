using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesSO", menuName = "EnemiesSO", order = 0)]
public class EnemiesSO : ScriptableObject
{
    public List<GameObject> _enemyPrefabs;

    public GameObject GetRandom()
    {
        return _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
    }
}