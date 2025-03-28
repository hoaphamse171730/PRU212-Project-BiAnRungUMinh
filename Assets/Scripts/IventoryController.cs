using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject iventoryPanel;
    public GameObject slotPrefabs;
    public int slotCount;
    public GameObject[] itemPrefabs;
    void Start()
        
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();

        Debug.Log("Total slots before creation: " + iventoryPanel.transform.childCount);

        if (iventoryPanel.transform.childCount == 0)
        {
            for (int i = 0; i < slotCount; i++)
            {
                Instantiate(slotPrefabs, iventoryPanel.transform);
            }
        }

        Debug.Log("Total slots after creation: " + iventoryPanel.transform.childCount);
    }
    public bool AddItem(GameObject itemPrefab)
    {
      foreach(Transform slotTransform in iventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Slot Count: " + slotCount);


        Debug.Log("inventory is full");
        return false;
    } 
    //public List<IventorySaveData> GetInventoryItem()
    //{
    //    List<IventorySaveData> inData = new List<IventorySaveData>();
    //    foreach (Transform slotTransform in iventoryPanel.transform)
    //    {
    //        Slot slot = slotTransform.GetComponent<Slot>();
    //        if (slot.currentItem != null)
    //        {
    //            Item item = slot.currentItem.GetComponent<Item>();
    //            inData.Add(new IventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
    //        }
    //    }
    //    return inData;
    //}
    //public void SetInventoryItem(List<IventorySaveData> iventorySaveDatas)
    //{
    //    foreach(Transform child in iventoryPanel.transform)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    for (int i = 0;i<slotCount; i++)
    //    {
    //        Instantiate(slotPrefabs, iventoryPanel.transform);
    //    }
    //    foreach (IventorySaveData data in iventorySaveDatas)
    //    {
    //        if (data.slotIndex < slotCount)
    //        {
    //            Slot slot = iventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
    //            GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
    //            if (itemPrefab != null)
    //            {
    //                GameObject item = Instantiate(itemPrefab, slot.transform);
    //                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    //                slot.currentItem = item;
    //            }
    //        }
    //    }
    //}
}
