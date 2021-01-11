using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public Camera dialogueCamera;
    public TMP_Text DialogueText;
    public GameObject[] ToDisable;
    public UnityEvent OnStartDialogue;
    public Queue<string> sentences;
    public string[] FromFileDialogue;
    [SerializeField]
    private TMP_Text _name;
    NPC npc;
    private void Start() 
    {
        sentences = new Queue<string>();   
    }
    private void Awake()
    {
        if (OnStartDialogue == null)
            OnStartDialogue = new UnityEvent();
        npc = GetComponent<NPC>();
    }

    public void say() 
    {
        UpdateFile();
        _name.text = transform.name;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Starting Convo");
        npc.agent.isStopped = true;
        npc.GetComponentInChildren<Animator>().enabled = false;
        
        npc.enabled = false;
       
        DialogueSystem.instance.Attach(this);
        sentences.Clear();
        foreach(string sentence in FromFileDialogue)
        {
            sentences.Enqueue(sentence);
        }
        DialogueSystem.instance.DisplayNextSentence(this);
        OnStartDialogue.Invoke();
    }


    public void EndDialogue()
    {
        Debug.Log("Done");
        DialogueSystem.instance.Detach();
        npc.GetComponentInChildren<Animator>().enabled = true;
        npc.enabled = true;
        npc.agent.isStopped = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnDestroy()
    {
        OnStartDialogue.RemoveAllListeners();
    }
    public void UpdateFile()
    {
        string path;
        path = "Assets/NPC dialogues/SentenceTesting.txt";
        StreamReader reader = new StreamReader(path);
        FromFileDialogue = File.ReadAllLines(path);
    }
}
