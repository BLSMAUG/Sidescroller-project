using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    public bool isInteractable;
    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable _interact))
        {
            _interact.Interact();
            isInteractable = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable _endInteract))
        {
            _endInteract.EndInteract();
            isInteractable = false;
        }
    }
}
