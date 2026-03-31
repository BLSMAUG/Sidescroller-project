using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : ObjectInteraction
{

    public bool IsOpened { get; private set;  }
    public string ChestId { get; private set; }
    public GameObject itemPrefab;
    public Sprite openedSprite;

    InputAction interactAction;

    void Start()
    {
        ChestId ??= GlobalHelper.GenerateUniqueId(gameObject);
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

        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        }
    }

    public void SetOpened(bool opened)
    {
        //IsOpened = opened;
        if (IsOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }



}
