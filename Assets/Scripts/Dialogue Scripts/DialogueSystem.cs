using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    public DialogueManager currentManager { get; private set; }
    private int index;
    private float _textSpeed = 2f;
    private string sentence;
    private bool _options = false;
    private bool _twoOptions = false;
    private bool _threeOptions = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Attach(DialogueManager dialogueManager)
    {
        currentManager = dialogueManager;
        dialogueManager.OnStartDialogue.AddListener(DialogueListener);
        currentManager.dialogueCamera.enabled = true;
        foreach (GameObject g in currentManager.ToDisable)
            g.SetActive(false);
    }

    private void DialogueListener()
    {
        
    }

    public void Detach()
    {
        if (currentManager != null)
        {
            currentManager.OnStartDialogue.RemoveListener(DialogueListener);
            currentManager.dialogueCamera.enabled = false;
            foreach (GameObject g in currentManager.ToDisable)
                g.SetActive(true);
        }
        currentManager = null;
    }
    public void DisplayNextSentence(DialogueManager dialogueManager)
    {
        if(dialogueManager.sentences.Count == 0)
        {
            dialogueManager.EndDialogue();
            return;
        }
        sentence = dialogueManager.sentences.Dequeue();
        StartCoroutine(Type(dialogueManager));
        Debug.Log(sentence);
    }

    IEnumerator Type(DialogueManager dialogueManager)
    {
        dialogueManager.DialogueText.text = "";
        foreach(char letter in sentence)
        {
            dialogueManager.DialogueText.text += letter;
            yield return new WaitForSeconds(_textSpeed*Time.deltaTime);
        }
    }

    public void OptionsTurnOn(DialogueManager dialogueManager)
    {
        if(_options == true && _twoOptions == true)
        {
            
        }
    }
}