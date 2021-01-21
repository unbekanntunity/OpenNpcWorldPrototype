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
    public TMP_Text DialogueText;
    public TMP_Text _name;
    public Button _option1;
    public Button _option2;
    public Button _option3;
    public Button _NewSentenceButton;

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

    public void say() 
    {
        player.playerCanMove = false;
        _isdialogue = true;
        UpdateFile();
        _name.text = npc.name;
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
        DisplayNextSentence();
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
        _NewSentenceButton.gameObject.SetActive(false);
        DialogueText.gameObject.SetActive(false);
        _name.gameObject.SetActive(false);
        _options = true;
        if (_options = true && _option2 == true)
        {
            _option1.gameObject.SetActive(true);
            _option2.gameObject.SetActive(true);
        }
        else if(_options = true && _3options == true)
        {
            _option1.gameObject.SetActive(true);
            _option2.gameObject.SetActive(true);
            _option3.gameObject.SetActive(true);
        }
    }
    public void DisplayNextSentence()
    {
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
        DialogueText.text = "";
        foreach(char letter in sentence)
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(_textSpeed*Time.deltaTime);
        }
    }
    public void Choices()
    {
        sentence = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        _option1.gameObject.SetActive(false);
        _option2.gameObject.SetActive(false);
        _option3.gameObject.SetActive(false);
        _NewSentenceButton.gameObject.SetActive(true);
        DialogueText.gameObject.SetActive(true);
        _name.gameObject.SetActive(true);
        DialogueText.text = sentence;
        _name.text = "Player";
        Debug.Log(sentence);
    }
}
