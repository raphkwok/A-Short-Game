using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractable : MonoBehaviour, Interactables
{

    public bool hovering;
    public Transform dialogue;
    public Animator indicator;
    public Transform dialoguePosition;
    public void OnStartHover()
    {
        print("start");
        hovering = true;
        if (!indicator.gameObject.activeInHierarchy) indicator.gameObject.SetActive(true);
        indicator.Play("Text Indicator Open");
    }

    public void OnInteract()
    {
        print("interact");
        DialogueManager.dialogueManager.dialoguePosition = dialoguePosition;
        DialogueManager.dialogueManager.dialogue = dialogue;
        DialogueManager.dialogueManager.StartDialogue();
        if (indicator.gameObject.activeInHierarchy) indicator.Play("Text Indicator Close");
    }

    public void OnEndHover()
    {
        print("end");
        hovering = false;
        indicator.Play("Text Indicator Close");
        if (!indicator.gameObject.activeInHierarchy) indicator.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (hovering)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                OnInteract();
            }
        }
    }
}
