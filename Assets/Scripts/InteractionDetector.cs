using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour, IInteractable
{

    public GameObject interactionIcon;

    public void Interact()
    {
        interactionIcon.SetActive(true);
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
