using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Helper data containers


    public Item Item;
    public int Count;

    ItemPickup()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<MeshFilter>().mesh = Item.ItemMesh;
        GetComponentInChildren<MeshRenderer>().material = Item.ItemMaterial;
        GetComponentInChildren<BoxCollider>().size = GetComponentInChildren<MeshFilter>().mesh.bounds.size;
        GetComponentInChildren<BoxCollider>().center = GetComponentInChildren<MeshFilter>().mesh.bounds.center;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPickedUp()
    {
        Debug.Log("Item picked up");
        Destroy(gameObject);
    }
}
