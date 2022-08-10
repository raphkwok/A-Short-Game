using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, Interactables
{

    public void OnStartHover()
    {
        print("start");
    }

    public void OnInteract()
    {
        print("interact");
    }

    public void OnEndHover()
    {
        print("end");
    }
}
