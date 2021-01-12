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
    public LayerMask ItemMask;

    // UI properties
    public GameObject InventoryPanel;
    Transform InventoryItemPanel;
    TMPro.TMP_Text InventoryStatusTextField;
    public GameObject ItemButtonPrefab;

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

        InventoryItemPanel = InventoryPanel.transform.GetChild(1);
        InventoryStatusTextField = InventoryPanel.transform.GetChild(2).GetComponent<TMPro.TMP_Text>();
        RefreshInventoryUI();
        InventoryPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForItemPickups();
        ManageInventoryUI();
    }

    void ClearItemButtons()
    {
        for(int i=0; i< InventoryItemPanel.childCount; i++)
        {
            GameObject.Destroy(InventoryItemPanel.GetChild(i).gameObject);
        }
    }

    void RefreshInventoryUI()
    {
        ClearItemButtons();
        foreach(ItemData i in InventoryList)
        {
            GameObject tempItem = Instantiate<GameObject>(ItemButtonPrefab);
            tempItem.GetComponent<ItemButtonHandler>().Init(i, this);
            tempItem.transform.SetParent(InventoryItemPanel);
        }
        InventoryStatusTextField.text = "Carrying : " + GetInventoryWeight() + ", Capacity : " + CarryCapacity.ToString();
    }

    void ManageInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryPanel.activeSelf){
                InventoryPanel.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                InventoryPanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
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

                RefreshInventoryUI();
                return;                                     // Item type exists in inventory. Add count to it and return.
            }
        }
        // At this point the item type does not exist already in the inventory list. So let's add it to the list.
        InventoryList.Add(new ItemData(PickedItem));

        RefreshInventoryUI();
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

                RefreshInventoryUI();
                return;                                     // Item type exists in inventory. Add count to it and return.
            }
        }
        // At this point the item type does not exist already in the inventory list. So let's add it to the list.
        InventoryList.Add(Itemdata);

        RefreshInventoryUI();
    }

    // Drops the item from the inventory and generates a pickup in the world to represent it.
    public void DropItem(int ItemId, int Count = 1)
    {
        Debug.Log("Dropped : " + ItemId);
        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i].Item.ItemId == ItemId)
            {
                // Remove the item from the inventory
                ItemData temp = InventoryList[i];
                int DropCount = System.Math.Min(temp.Count, Count);
                temp.Count -= DropCount;
                InventoryList[i] = temp;

                // Refresh item inventory and refresh UI
                if (InventoryList[i].Count == 0) InventoryList.RemoveAt(i);
                RefreshInventoryUI();

                // Spawn the removed item into the world as a pickup
                ItemManager.instance.GenerateItemFromId(ItemId, Camera.main.transform.position+ Camera.main.transform.forward * 1, new Quaternion() , DropCount);
                return;
            }
        }
    }

    public int FindItemSlot(Item Item)
    {
        for(int i = 0;  i < InventoryList.Count; i++)
        {
            if(InventoryList[i].Item == Item)
            {
                return i;
            }
        }
        return -1;
    }

    public void SwitchItems(Item Item1, Item Item2)
    {
        Debug.Log("Switching");
        int Item1Index = FindItemSlot(Item1);
        int Item2Index = FindItemSlot(Item2);
        if(Item1Index == -1 && Item2Index == -1)
        {
            return;
        }
        ItemData Item1Data = InventoryList[Item1Index];
        ItemData Item2Data = InventoryList[Item2Index];
        InventoryList[Item2Index] = Item1Data;
        InventoryList[Item1Index] = Item2Data;

        RefreshInventoryUI();
    }

    public void UseItem(ItemData Itemdata)
    {
        Itemdata.Item.OnItemUsed();
        Debug.Log("ItemUsed");
    }

    void CraftItem(ItemData ItemToCraft)
    {

    }
}
