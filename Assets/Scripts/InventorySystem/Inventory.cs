using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public struct ItemData
    {
        public Item Item;
        public int Count;

        public ItemData(Item Item, int Count)
        {
            this.Item = Item;
            this.Count = Count;
        }
        public ItemData(ItemPickup PickUp)
        {
            this.Item = PickUp.Item;
            this.Count = PickUp.Count;
        }
    };

    // Properties
    public float CarryCapacity = 10;
    List<ItemData> InventoryList = new List<ItemData>();
    public ItemManager ItemManager;
    public LayerMask ItemMask;

    // properties from PlayerActionsScript
    KeyCode InteractButton;
    float InteractionRange;
    Camera PlayerCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Get required properties from player actions
        PlayerActions PA = GetComponent<PlayerActions>();
        InteractButton = PA.InteractButton;
        InteractionRange = PA.InteractionRange;
        PlayerCamera = PA.PlayerCamera;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForItemPickups();
    }

    void CheckForItemPickups()
    {
        if (Input.GetKeyDown(InteractButton))
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, InteractionRange, ItemMask))
            {
                ItemPickup PickUp = hit.transform.GetComponentInParent<ItemPickup>();
                if (PickUp == null)
                    PickUp = hit.transform.GetComponentInChildren<ItemPickup>();
                if (PickUp == null)
                {
                    return;
                }
                PickUpItem(PickUp);
                return;
            }
        }
    }

    void PickUpItem(ItemPickup PickedItem)
    {
        if (CanTakeItem(PickedItem.Item, PickedItem.Count))
        {
            AddItem(PickedItem);
            PickedItem.OnPickedUp();
        }
    }

    bool CanTakeItem(Item Item, int Count)
    {
        return GetInventoryWeight()+Item.Weight*Count <= CarryCapacity;
    }

    float GetInventoryWeight()
    {
        float CarryingMass = 0.0f;
        foreach (ItemData i in InventoryList)
        {
            CarryingMass += i.Item.Weight * i.Count;        // weight of item * count of that item is the total wieght of that slot
        }
        return CarryingMass;
    }

    void AddItem(ItemPickup PickedItem)
    {
        for(int i = 0; i<InventoryList.Count; i++)
        {
            if(InventoryList[i].Item.ItemId == PickedItem.Item.ItemId)
            {
                ItemData temp= InventoryList[i];
                temp.Count+= PickedItem.Count;
                InventoryList[i]= temp;
                return;                                     // Item type exists in inventory. Add count to it and return.
            }
        }
        // At this point the item type does not exist already in the inventory list. So let's add it to the list.
        InventoryList.Add(new ItemData(PickedItem));
    }

    void AddItem(ItemData Itemdata)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i].Item.ItemId == Itemdata.Item.ItemId)
            {
                ItemData temp = InventoryList[i];
                temp.Count += Itemdata.Count;
                InventoryList[i] = temp;
                return;                                     // Item type exists in inventory. Add count to it and return.
            }
        }
        // At this point the item type does not exist already in the inventory list. So let's add it to the list.
        InventoryList.Add(Itemdata);
    }
}
