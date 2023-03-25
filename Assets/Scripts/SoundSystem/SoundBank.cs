using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundBank", menuName = "SoundBank", order = 0)]
public class SoundBank : ScriptableObject
{
    public List<SoundData> data;
}