using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    public bool isInteractable = false;
    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable _interact))
        {
            isInteractable = true;
            _interact.Interact();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable _endInteract))
        {
            isInteractable = false;
            _endInteract.EndInteract();
        }
    }
}
