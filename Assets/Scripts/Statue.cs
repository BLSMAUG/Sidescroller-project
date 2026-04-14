using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Statue : ObjectInteraction
{
    InputAction interactAction;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (isInteractable == true)
        {
            if (interactAction.IsPressed())
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
