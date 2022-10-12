using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractable : MonoBehaviour, Interactables
{

    public bool hovering;
    public Transform dialogue;
    public void OnStartHover()
    {
        print("start");
        hovering = true;
    }

    public void OnInteract()
    {
        print("interact");
        DialogueManager.dialogueManager.dialogue = dialogue;
        DialogueManager.dialogueManager.StartDialogue();
    }

    public void OnEndHover()
    {
        print("end");
        hovering = false;

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
