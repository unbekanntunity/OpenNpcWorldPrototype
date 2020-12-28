using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public KeyCode InteractButton = KeyCode.E;

    public LayerMask Mask;
    public float InteractionRange;

    public Camera PlayerCamera;

    private void Update()
    {
        if (Input.GetKeyDown(InteractButton))
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit,InteractionRange, Mask))
            {
                DialogueManager dialogue = hit.transform.GetComponentInParent<DialogueManager>();
                if(dialogue==null)
                    dialogue = hit.transform.GetComponentInChildren<DialogueManager>();
                if (dialogue == null)
                    return;
                Vector3 rot = dialogue.transform.eulerAngles;
                dialogue.transform.LookAt(transform);
                dialogue.transform.eulerAngles = new Vector3(rot.x, dialogue.transform.eulerAngles.y, rot.z);
                dialogue.say("Hello there. How are you");
            }
        }
    }
}
