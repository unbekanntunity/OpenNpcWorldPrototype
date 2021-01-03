using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHandler : MonoBehaviour
{
    Inventory.ItemData ItemData;
    Inventory Inventory;

    bool bIsDragged = false;
    Vector3 Position;
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
            if (LastCollidedObject)
            {
                Debug.Log("ToDrop");
            }
            else transform.position = Position;
            bIsDragged = false;
        }
    }

    // Inteded for use by special UI sreas like the drop field, destroy field, etc
    public void OnDrop()
    {
        if (LastCollidedObject)
        {
            ItemButtonHandler temp = LastCollidedObject.GetComponent<ItemButtonHandler>();
            if (temp)
            {
                temp.Inventory.DropItem(temp.ItemData.Item.ItemId);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ColEnter");
        LastCollidedObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("ColExit");
        LastCollidedObject = null;
    }
}
