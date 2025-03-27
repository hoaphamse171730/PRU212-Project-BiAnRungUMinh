using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable InteractableRange = null;
    public GameObject interactionIcon;
    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Sự kiện OnInteract đã được gọi!");
        if (context.performed)
        {
            

            if (InteractableRange != null)
            {
                Debug.Log("Tương tác với: " + InteractableRange);
                InteractableRange.Interact();
            }
            else
            {
                Debug.Log("Không có đối tượng nào để tương tác!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            InteractableRange = interactable;
            interactionIcon.SetActive(true);
            Debug.Log("Đã vào vùng tương tác với: " + other.gameObject.name);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable == InteractableRange)
        {
            InteractableRange = null;
            interactionIcon.SetActive(false);
        }
    }
}