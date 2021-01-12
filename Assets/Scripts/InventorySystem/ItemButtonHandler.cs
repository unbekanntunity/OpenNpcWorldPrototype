using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHandler : MonoBehaviour
{
    Inventory.ItemData ItemData;
    Inventory Inventory;

    bool bIsDragged = false;
    public Vector3 Position;
    GameObject LastCollidedObject = null;

    public ItemButtonHandler(Inventory.ItemData ItemData)
    {
        this.ItemData = ItemData;
    }

    public void Init(Inventory.ItemData Itemdata, Inventory Inventory)
    {
        this.ItemData = Itemdata;
        this.Inventory = Inventory;
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
        transform.position = Input.mousePosition;
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
                Debug.Log(LastCollidedObject.name);
                ItemButtonHandler temp = LastCollidedObject.GetComponent<ItemButtonHandler>();
                if (temp) Debug.Log(temp.ItemData.Item.ItemName);
                // If the buttonhandler of the object has an item defined, it is an inventory button
                if (temp && temp.ItemData.Item)
                {
                    Inventory.SwitchItems(ItemData.Item, temp.ItemData.Item);
                }
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
                if (temp.ItemData.Item) temp.Inventory.DropItem(temp.ItemData.Item.ItemId);
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
