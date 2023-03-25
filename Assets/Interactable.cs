using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnter;
    [SerializeField] private UnityEvent _onExit;
    [SerializeField] private UnityEvent _onInteraction;

    public UnityEvent OnInteraction
    {
        get
        {
            return _onInteraction;
        }
    }

    private bool _canInteract = false;
    
    private void OnTriggerEnter(Collider other)
    {
        _canInteract = true;
        _onEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _canInteract = false;
        _onExit.Invoke();
    }

    private void Update()
    {
        if (!_canInteract)
            return;

        if (Input.GetKey(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        _onInteraction?.Invoke();
    }
}
