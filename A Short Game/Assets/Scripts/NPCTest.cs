using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTest : MonoBehaviour, Interactables
{
    public void OnStartHover()
    {
        print("Start");
    }

    public void OnInteract()
    {
        print("Interact");
    }

    public void OnEndHover()
    {
        print("End");
    }
}
