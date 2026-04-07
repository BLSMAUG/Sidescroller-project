using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : ObjectInteraction
{

    public bool IsOpened { get; private set;  }
    //public string ChestId { get; private set; }
    public Sprite openedSprite;

    InputAction interactAction;

    void Start()
    {
        //ChestId ??= GlobalHelper.GenerateUniqueId(gameObject);
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (isInteractable == true)
        {
            if (interactAction.IsPressed())
            {
                OpenChest();
            }
        }
    }

    private void OpenChest()
    {
        SetOpened(true);
    }

    public void SetOpened(bool opened)
    {
        //IsOpened = opened;
        if (IsOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            
        }
        isInteractable = false;
    }



}
