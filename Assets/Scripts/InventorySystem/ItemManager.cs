using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> ItemDB = new List<Item>()
    {

    };

    Item InvalidItem;
    public GameObject ItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {

    }

    public Item GetItemFromId(int ItemId)
    {
        Item Item;
        foreach(Item i in ItemDB)
        {
            if(i.ItemId == ItemId)
            {
                Item = i;
                return Item;
            }
        }
        return InvalidItem;
    }

    public GameObject GenerateItemFromId(int ItemId, Vector3 Position, Quaternion Rotation)
    {
        Item item = GetItemFromId(ItemId);
        GameObject tempItem = Instantiate<GameObject>(ItemPrefab, Position, Rotation);
        Item TempBase = tempItem.GetComponent<Item>();
        TempBase = item;
        return tempItem;
    }
}
