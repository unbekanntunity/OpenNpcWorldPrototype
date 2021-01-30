using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerActions : MonoBehaviour
{
    public KeyCode InteractButton = KeyCode.E;
    public LayerMask Mask;
    public float InteractionRange;
    public Camera PlayerCamera;

    public GameObject QuestUiWindow;
    private bool questWindowActive = false;
    public Quest quest;
    public bool _indialogue = false;
    private RaycastHit _currenthit;

    private void Update()
    {
        if (Input.GetKeyDown(InteractButton))
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit,InteractionRange, Mask))
            {
                _currenthit = hit;
                DialogueManager dialogue = hit.transform.GetComponentInParent<DialogueManager>();
                if(dialogue==null)
                    dialogue = hit.transform.GetComponentInChildren<DialogueManager>();
                if (dialogue == null)
                    return;
                if(dialogue._isdialogue == false)
                {
                    _indialogue = true;
                    Vector3 rot = dialogue.transform.eulerAngles;
                    dialogue.transform.LookAt(transform);
                    dialogue.transform.eulerAngles = new Vector3(rot.x, dialogue.transform.eulerAngles.y, rot.z);
                    dialogue.say();
                }
            }
        }
        if(_indialogue == true)
        {
            PressSpeakButton(_currenthit.transform.GetComponentInParent<DialogueManager>());
        }
        QuestWindowToggle();
    }

    private void QuestWindowToggle()
    {

        if (Input.GetKeyDown(KeyCode.X) && questWindowActive == false)
        {

            QuestUiWindow.SetActive(true);
            questWindowActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetKeyDown(KeyCode.X) && questWindowActive == true)
        {
            QuestUiWindow.SetActive(false);
            questWindowActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void PressSpeakButton(DialogueManager dialogue)
    {
        var pointer = new PointerEventData(EventSystem.current);
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse 0 pressed");
            ExecuteEvents.Execute(dialogue._speak.gameObject, pointer, ExecuteEvents.submitHandler);
        }
        if(Input.GetKeyDown(KeyCode.Space) && dialogue.displayingdialogue == false)
        {
            dialogue.DisplayNextSentence(_currenthit.transform.gameObject);
        }
    }
}
