using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bundle : MonoBehaviour
{
    [SerializeField]
    public List<Inventory.ItemData> Items;

    // PickUp components
    Canvas PickUpCanvas;
    MeshFilter PickUpMesh;
    MeshRenderer PickUpRenderer;
    BoxCollider PickUpCollider;

    Bundle()
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

    public void OnPickedUp()
    {
        Destroy(gameObject);
    }
}
