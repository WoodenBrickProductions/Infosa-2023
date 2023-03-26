using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEPool : MonoBehaviour
{
    [SerializeField] int startSize;
    [SerializeField] GameObject effectGO;
    [SerializeField] AOEEffect[] effects;

    [SerializeField] float effectTimer = 10;
    int current;

    private void Awake()
    {
        if (startSize <= 0)
            startSize = 20;

        effects = new AOEEffect[startSize];
        for (int i = 0; i < effects.Length; i++)
        {
            var go = Instantiate(effectGO, gameObject.transform);
            go.SetActive(false);
            var effect = go.GetComponent<AOEEffect>();
            effect.pool = this;
            effects[i] = effect;
        }

        effectGO.SetActive(false);
    }

    public void StartEffect(Vector3 position, float radius, Color color)
    {
        var effect = effects[current++];
        if (current == effects.Length)
            current = 0;

        effect.duration = effectTimer;
        effect.color = color;
        effect.radius = radius;
        effect.transform.position = position;
        effect.Activate();
    }
}
