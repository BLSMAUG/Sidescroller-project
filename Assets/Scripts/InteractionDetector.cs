using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : ObjectInteraction, IInteractable
{

    public GameObject interactionIcon;

    public void Interact()
    {
        if (isInteractable == true)
        {
            interactionIcon.SetActive(true);
        }
    }

    public void EndInteract()
    {
        interactionIcon.SetActive(false);
    }

    void Start()
    {
        interactionIcon.SetActive(false);
    }

}
