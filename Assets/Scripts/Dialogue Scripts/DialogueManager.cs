using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public Camera dialogueCamera;
    public GameObject[] ToDisable;
    public UnityEvent OnStartDialogue;
    public Queue<string> sentences;
    public string[] FromFileDialogue;
    [SerializeField]
    private bool _options = false;
    public bool _2options = false;
    public bool _3options = false;
    private int index;
    private float _textSpeed = 2f;
    private string sentence;
    public bool _isdialogue = false;
    private FirstPersonAIO player;
    private PlayerActions _playeractions;
    public bool displayingdialogue = false;
    public bool _speak = false;
    private Dialogue _dialogue;

    NPC npc;
    private void Start() 
    {
        sentences = new Queue<string>();
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonAIO>();
    }
    private void Awake()
    {
        if (OnStartDialogue == null)
            OnStartDialogue = new UnityEvent();
        npc = GetComponent<NPC>();
    }

    public void say(GameObject caller) 
    {
        _playeractions = GameObject.FindWithTag("Player").GetComponent<PlayerActions>();
        _dialogue = _playeractions.dialogue_gameobject.GetComponent<Dialogue>();
        if(caller == null)
        {
            Debug.Log("Dialogue is null");
        }
        player.playerCanMove = false;
        _isdialogue = true;
        UpdateFile();
        _dialogue._name.text = caller.name;
        Debug.Log(caller.name);
        player.lockAndHideCursor = false;
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
        DisplayNextSentence(caller.gameObject);
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
        _isdialogue = false;
        player.playerCanMove = true;
        _playeractions = GameObject.FindWithTag("Player").GetComponent<PlayerActions>();
        _playeractions._indialogue = false;
        _playeractions.dialogue_gameobject.SetActive(false);
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
    public void OptionsActive()
    {
        Debug.Log("Player Options started");
        _dialogue.DialogueText.gameObject.SetActive(false);
        _dialogue._name.gameObject.SetActive(false);
        _options = true;
        if (_options = true && _2options == true)
        {
            _dialogue._option1.gameObject.SetActive(true);
            _dialogue._option1.interactable = true;
            _dialogue._option2.gameObject.SetActive(true);
            _dialogue._option2.interactable = true;
        }
        else if(_options = true && _3options == true)
        {
            _dialogue._option1.gameObject.SetActive(true);
            _dialogue._option1.interactable = true;
            _dialogue._option2.gameObject.SetActive(true);
            _dialogue._option2.interactable = true;
            _dialogue._option3.gameObject.SetActive(true);
            _dialogue._option3.interactable = true;
        }
        _dialogue._option1.onClick.AddListener(Choices);
        _dialogue._option2.onClick.AddListener(Choices);
        _dialogue._option3.onClick.AddListener(Choices);

    }
    public void DisplayNextSentence(GameObject caller)
    {
        Debug.Log(caller.name);
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        sentence = sentences.Dequeue();
        StartCoroutine(Type());
        Debug.Log(sentence);
    }

    IEnumerator Type()
    {
        displayingdialogue = true;
        _dialogue.DialogueText.text = "";
        foreach(char letter in sentence)
        {
            _dialogue.DialogueText.text += letter;
            yield return new WaitForSeconds(_textSpeed*Time.deltaTime);
        }
        displayingdialogue = false;
    }
    public void Choices()
    {
        sentence = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        _dialogue._option1.gameObject.SetActive(false);
        _dialogue._option1.interactable = false;
        _dialogue._option2.gameObject.SetActive(false);
        _dialogue._option2.interactable = false;
        _dialogue._option3.gameObject.SetActive(false);
        _dialogue._option3.interactable = false;
        _dialogue.DialogueText.gameObject.SetActive(true);
        _dialogue._name.gameObject.SetActive(true);
        _dialogue.DialogueText.text = sentence;
        _dialogue._name.text = "Player";
        _speak = false;
        displayingdialogue = false;
    }
}
