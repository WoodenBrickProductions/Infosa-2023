using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEEffect : MonoBehaviour
{
    public Color color;
    public float duration;
    public AOEPool pool;
    public float radius;
    private float timer;

    MeshRenderer renderer;

    public void Activate()
    {
        gameObject.SetActive(true);
        enabled = true;
        color.a = 0.5f;
        renderer.material.color = color;
        timer = duration;
        transform.parent = null;
        transform.localScale = new Vector3(radius, radius, radius);
    }

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            var color = renderer.material.color;
            color.a -= Time.deltaTime / duration / 2;
            radius -= Time.deltaTime / duration;
            transform.localScale = new Vector3(radius, radius, radius);
            renderer.material.color = color;

            timer -= Time.deltaTime;
            return;
        }

        Deactivate();
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
        enabled = false;
        transform.parent = pool.transform;
    }
}
