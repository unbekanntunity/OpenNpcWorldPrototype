using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHandler : MonoBehaviour
{
    Inventory.ItemData ItemData;
    Inventory Inventory;
    public bool bIsItemButton = true;

    bool bIsDragged = false;
    Vector3 Position;
    GameObject LastCollidedObject = null;

    public ItemButtonHandler(Inventory.ItemData ItemData)
    {
        this.ItemData = ItemData;
    }

    public void Init(Inventory.ItemData Itemdata, Inventory Inventory)
    {
        this.Inventory = Inventory;

        if (!Itemdata.Item)
        {
            // If this is null button destroy all children as they are not of use
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
            return;
        }
        this.ItemData = Itemdata;
        GetComponentInChildren<TMPro.TMP_Text>().text = ItemData.Item.ItemName;
        GetComponent<Image>().sprite = ItemData.Item.ItemImage;
        GetComponentsInChildren<TMPro.TMP_Text>()[1].text = ItemData.Count.ToString();
    }

    public void OnButtonClicked()
    {
        if(!bIsDragged) Inventory.UseItem(ItemData);
    }

    public void OnDrag()
    {
        transform.position = Input.mousePosition;// + new Vector3(0, 0, -10);
    }

    public void OnBeginDrag()
    {
        Position = transform.position;
        bIsDragged = true;
    }

    public void OnEndDrag()
    {
        if (bIsDragged)
        {
            if (LastCollidedObject)  // If there is a last collided object
            {
                ItemButtonHandler temp = LastCollidedObject.GetComponent<ItemButtonHandler>();
                
                if (temp && temp.bIsItemButton)
                {
                    Inventory.SwitchItems(transform.parent.GetSiblingIndex(), temp.transform.parent.GetSiblingIndex());
                }
                else transform.position = Position;
            }
            else transform.position = Position;
            StartCoroutine(Dragged());
        }
    }
    
    IEnumerator Dragged() 
    {
        yield return null;
        bIsDragged = false;
    }

    // Inteded for use by special UI areas like the drop field, destroy field, etc
    public void OnDrop()
    {
        if (LastCollidedObject)
        {
            ItemButtonHandler temp = LastCollidedObject.GetComponent<ItemButtonHandler>();
            if (temp)
            {
                Debug.Log("Dropping");
                if (temp.ItemData.Item) temp.Inventory.DropItem(temp.ItemData.Item);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LastCollidedObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LastCollidedObject = null;
    }
}
