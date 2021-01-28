using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    public List<Inventory.ItemData> Items;

    // PickUp components
    Canvas PickUpCanvas;
    MeshFilter PickUpMesh;
    MeshRenderer PickUpRenderer;
    BoxCollider PickUpCollider;

    // UI
    public GameObject ChestPanel;
    Transform ChestItemPanel;
    public int MaxNumSlots = 12;
    public GameObject ItemSlotPrefab;

    Chest()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialise mesh
        PickUpMesh = GetComponentInChildren<MeshFilter>();
        PickUpRenderer = GetComponentInChildren<MeshRenderer>();
        PickUpCollider = GetComponentInChildren<BoxCollider>();
        PickUpCollider.size = PickUpMesh.mesh.bounds.size;
        PickUpCollider.center = PickUpMesh.mesh.bounds.center;

        // Initialise canvas
        PickUpCanvas = GetComponentInChildren<Canvas>();
        PickUpCanvas.transform.localPosition = new Vector3(0, 0.5f + PickUpCollider.size.y * PickUpCollider.transform.localScale.y, 0);

        Items.Clear();
        for (int i = 0; i < MaxNumSlots; i++)
        {
            Items.Add(new Inventory.ItemData(null, 0));
        }
        ChestItemPanel = ChestPanel.transform.GetChild(1);
        ChestPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ManageCanvas();
    }

    void ManageCanvas()
    {
        if ((Camera.main.transform.position - transform.position).magnitude < 5.0f)
        {

        }
        PickUpCanvas.transform.LookAt(Camera.main.transform.position);
    }

    void ClearItemButtons()
    {
        for (int i = 0; i < ChestItemPanel.childCount; i++)
        {
            GameObject.Destroy(ChestItemPanel.GetChild(i).gameObject);
        }
    }

    void RefreshUI()
    {
        ClearItemButtons();
        foreach (Inventory.ItemData i in Items)
        {
            GameObject tempSlot = Instantiate<GameObject>(ItemSlotPrefab);
            GameObject tempItem = tempSlot.transform.GetChild(0).gameObject;
            tempItem.GetComponent<ItemButtonHandler>().Init(i, this);
            tempSlot.transform.SetParent(ChestItemPanel);
        }

    }

    public void OnOpened()
    {
        ChestPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        RefreshUI();
    }

    public void OnClosed()
    {
        ChestPanel.SetActive(false);
    }

    public void OnTakeItem(Inventory.ItemData Slot)
    {
        int SlotIndex = FindItem(Slot.Item, Slot.Count);
        if (SlotIndex != -1)
        {
            Items.RemoveAt(SlotIndex);
        }
    }

    public void OnStoreItem(Inventory.ItemData Slot, int Index)
    {
        // If no item is defined in the destination slot, store it in that slot
        if (!Items[Index].Item)
        {
            Items[Index] = Slot;
        }
        else
        {
            AddItem(Slot);
        }
        RefreshUI();
    }

    void AddItem(Inventory.ItemData Itemdata)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Item && Items[i].Item.ItemId == Itemdata.Item.ItemId)
            {
                Inventory.ItemData temp = Items[i];
                temp.Count += Itemdata.Count;
                Items[i] = temp;

                RefreshUI();
                return;                                     // Item type exists in inventory. Add count to it and return.
            }
        }
        // At this point the item type does not exist already in the inventory list. So let's add it to the list.
        AddToFreeSlot(Itemdata);
    }

    void AddToFreeSlot(Inventory.ItemData ItemData)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].Item)     // Found an empty slot
            {
                Items[i] = ItemData;

                RefreshUI();
                return;                                     // Item type exists in inventory. Add count to it and return.
            }
        }
    }

    int FindItem(Item Item, int Count = 1)
    {
        for(int i=0; i<Items.Count; i++)
        {
            if(Items[i].Item == Item && Items[i].Count >= Count)
            {
                return i;
            }
        }
        return -1;
    }

}
