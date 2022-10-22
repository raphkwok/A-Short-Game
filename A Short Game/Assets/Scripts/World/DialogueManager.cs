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
    public Dialogue dialogueText;

    public Transform dialoguePosition;
    public bool dialogueActive;

    [Header("Dialogue Choice Settings")]
    public int dialogueChoice;

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
        PlayerManager.player.dialoguePosition = dialoguePosition;
        PlayerManager.player.state = PlayerManager.State.Dialogue;
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
        dialogueText = dialogue.GetComponent<DialogueObject>().dialogue;
        dialogueText.gameObject.SetActive(true);

        // Start dialogue
        textComplete = false;
        dialogueActive = true;

    }

    public void Next()
    {
        // Debug.Log("Next");
        if (dialogueText.isChoice)
        {
            IsChoice();
            return;
        }

        // If text is not a choice
        if (textComplete && !textDisappeared)
        {
            // Disappear text
            dialogueText.GetComponent<TextAnimatorPlayer>().StartDisappearingText();
        }
        else if (textComplete && textDisappeared)
        {
            textComplete = false;
            textDisappeared = false;

            if (dialogueText.nextOptions.Length == 0)
            {
                EndDialogue();
            }
            else
            {
                dialogueText.gameObject.SetActive(false);
                dialogueText = dialogueText.nextOptions[0];
                dialogueText.gameObject.SetActive(true);

                if (dialogueText.isChoice) IsChoice();
            }
        }
        else
        {
            textComplete = true;

            dialogueText.GetComponent<TextAnimatorPlayer>().SkipTypewriter();
        }
    }

    public void IsChoice()
    {
        if (!dialogueText.showing && !dialogueText.opened)
        {
            StartCoroutine(dialogueText.Show());
            dialogueChoice = 0;
        }
        else if (dialogueText.showed && dialogueText.showing)
        {
            // print("yes");
            // TODO use better key system
            if (Keyboard.current[Key.W].wasPressedThisFrame || Keyboard.current[Key.S].wasPressedThisFrame)
            {

                print("w or s");
                if (Keyboard.current[Key.W].wasPressedThisFrame)
                {
                    dialogueChoice++;
                }
                else
                {
                    dialogueChoice--;
                }
                dialogueChoice = (int)Mathf.Repeat(dialogueChoice, dialogueText.nextOptions.Length);
            }
            else if (Keyboard.current[Key.E].wasPressedThisFrame)
            {
                // print("e");
                // Record text option
                dialogueText.StartCoroutine(dialogueText.Hide());
            }
        }
        else if (!dialogueText.showed && !dialogueText.showing)
        {
            dialogueText.opened = false;
            textComplete = false;
            textDisappeared = false;

            if (dialogueText.nextOptions.Length == 0)
            {
                EndDialogue();
            }
            else
            {
                dialogueText = dialogueText.nextOptions[dialogueChoice];
                dialogueText.gameObject.SetActive(true);
            }
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
        dialogueText.gameObject.SetActive(false);


        dialogue.GetChild(1).GetComponent<Animator>().Play("Dialogue Box Close");

        PlayerManager.player.state = PlayerManager.State.Active;
        // dialogue.gameObject.SetActive(false);
        StartCoroutine(EndCycle());
    }

    IEnumerator EndCycle()
    {
        yield return new WaitForSeconds(0.5f);
        dialogue.gameObject.SetActive(false);
    }
}
