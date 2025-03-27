using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject interfacefab;
    public Sprite openedSprite;


    void Start()
    {
        ChestID ??=GlobalHelper.GenerateUniqueID(gameObject);
    }


    public bool CanInteract()
    {
        return !IsOpened;
    }
    public void Interact()
    {
       if(!CanInteract()) return;
        OpenChest();
        //open chest
    }
    private void OpenChest()
    {
        SetOpened(true);
        if (interfacefab )
        {
            GameObject dropItem = Instantiate(interfacefab, transform.position + Vector3.down, Quaternion.identity);

            // Đảm bảo vật phẩm có Rigidbody2D



            // Random hướng (-1 là trái, 1 là phải)
            float direction = Random.Range(-1f, 1f);

            // Đẩy item bay theo hướng ngẫu nhiên

        }
    }
    public void SetOpened(bool Opened)
    {
      
        if (IsOpened = Opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

}

internal interface IInteractable
{
    bool CanInteract();
    void Interact();
}