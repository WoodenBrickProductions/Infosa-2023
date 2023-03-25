using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "LevelsSO", menuName = "LevelsSO", order = 0)]
    public class LevelsSO : ScriptableObject
    {
        public List<GameObject> Levels;

        public GameObject GetRandom()
        {
            return Levels[Random.Range(0, Levels.Count)];
        }
    }
}