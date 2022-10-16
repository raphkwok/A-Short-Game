using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Febucci.UI;
using Cinemachine;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dialogueManager;

    [Header("Dialogue Settings")]
    public bool textComplete;
    public bool textDisappeared;
    public int dialogueIndex;
    public Transform dialogue;
    public Transform dialogueText;

    public bool dialogueActive;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = this;
        dialogueIndex = 100;
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
        if (dialogueActive) return;
        if (dialogueIndex != 0) StartCoroutine(StartSequence());

        // Reset text stats
        dialogueIndex = 0;
    }

    IEnumerator StartSequence()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // Open Dialogue box
        dialogue.GetChild(1).gameObject.SetActive(true);
        dialogue.GetChild(1).GetComponent<Animator>().Play("Dialogue Box Open");
        yield return new WaitForSeconds(0.5f);
        dialogue.gameObject.SetActive(true);

        // Enable camera
        dialogue.GetChild(0).GetComponent<CinemachineVirtualCamera>().Priority = 100;

        // Get reference to dialogue text and enable
        dialogueText = dialogue.GetComponent<DialogueObject>().TextObject;
        dialogueText.GetChild(dialogueIndex).gameObject.SetActive(true);

        // Start dialogue
        textComplete = false;
        dialogueActive = true;

    }

    void Next()
    {
        if (textComplete && !textDisappeared)
        {
            // Disappear text
            dialogueText.GetChild(dialogueIndex).GetComponent<TextAnimatorPlayer>().StartDisappearingText();
        }
        else if (textComplete && textDisappeared)
        {
            dialogueIndex++;
            textComplete = false;
            textDisappeared = false;

            if (dialogueIndex >= dialogueText.childCount)
            {
                EndDialogue();
            }
            else
            {
                dialogueText.GetChild(dialogueIndex).gameObject.SetActive(true);
                dialogueText.GetChild(dialogueIndex - 1).gameObject.SetActive(false);
            }
        }
        else
        {
            textComplete = true;

            dialogueText.GetChild(dialogueIndex).GetComponent<TextAnimatorPlayer>().SkipTypewriter();
        }
    }

    public void CompletedText()
    {
        textComplete = true;
    }

    public void DisappearedText()
    {
        textDisappeared = true;
        Next();
    }
    void EndDialogue()
    {

        Cursor.lockState = CursorLockMode.Confined;
        // Set variables
        dialogueActive = false;

        // Disable camera
        dialogue.GetChild(0).GetComponent<CinemachineVirtualCamera>().Priority = 0;

        // Diable text
        dialogueText.GetChild(dialogueIndex - 1).gameObject.SetActive(false);


        dialogue.GetChild(1).GetComponent<Animator>().Play("Dialogue Box Close");
        // dialogue.gameObject.SetActive(false);
        StartCoroutine(EndCycle());
    }

    IEnumerator EndCycle()
    {
        yield return new WaitForSeconds(0.5f);
        dialogue.gameObject.SetActive(false);
    }
}
