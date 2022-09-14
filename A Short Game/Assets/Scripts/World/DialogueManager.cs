using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Febucci.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dialogueManager;

    bool textComplete;
    public int dialogueIndex;
    public Transform dialogue;

    public bool dialogueActive;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame && dialogueActive)
        {
            Next();
        }
    }

    public void StartDialogue()
    {
        dialogueIndex = 0;
        textComplete = false;
        dialogueActive = true;


        dialogue.GetChild(dialogueIndex).gameObject.SetActive(true);
    }

    void Next()
    {
        if (textComplete)
        {
            dialogueIndex++;
            textComplete = false;

            if (dialogueIndex >= dialogue.childCount)
            {
                EndDialogue();
            }
            else
            {
                dialogue.GetChild(dialogueIndex).gameObject.SetActive(true);
                dialogue.GetChild(dialogueIndex - 1).gameObject.SetActive(false);
            }
        }
        else
        {
            textComplete = true;

            dialogue.GetChild(dialogueIndex).GetComponent<TextAnimatorPlayer>().SkipTypewriter();
        }
    }

    public void CompletedText()
    {
        textComplete = true;
    }
    void EndDialogue()
    {
        dialogueActive = false;
        dialogue.GetChild(dialogueIndex - 1).gameObject.SetActive(false);
        // dialogue.gameObject.SetActive(false);
    }
}
