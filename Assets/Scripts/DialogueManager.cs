using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Camera dialogueCamera;
    public TMP_Text DialogueText;
    public GameObject[] ToDisable;
    public UnityEvent OnStartDialogue;
    NPC npc;
    private void Awake()
    {
        if (OnStartDialogue == null)
            OnStartDialogue = new UnityEvent();
        npc = GetComponent<NPC>();
    }

    public void say(string s)
    {
        string[] array = s.Split('.');
        StartCoroutine(talk(array));
        OnStartDialogue.Invoke();
    }

    IEnumerator talk( string[] str)
    {
        DialogueText.text = "";
        npc.agent.isStopped = true;
        npc.GetComponentInChildren<Animator>().enabled = false;
        
        npc.enabled = false;
       
        DialogueSystem.instance.Attach(this);
        foreach (string s in str)
        {
           
            yield return new WaitForSeconds(0.3f);
            DialogueText.text = s;
            yield return new WaitForSeconds(1f);
           
        }
        DialogueSystem.instance.Detach();
        npc.GetComponentInChildren<Animator>().enabled = true;
        npc.enabled = true;
        npc.agent.isStopped = false;
    }

    private void OnDestroy()
    {
        OnStartDialogue.RemoveAllListeners();
    }
}
