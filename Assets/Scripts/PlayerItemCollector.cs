using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private IventoryController iventoryController;
    void Start()
    {
        iventoryController = FindAnyObjectByType<IventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                bool itemAdded = iventoryController.AddItem(collision.gameObject);
                if (itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
