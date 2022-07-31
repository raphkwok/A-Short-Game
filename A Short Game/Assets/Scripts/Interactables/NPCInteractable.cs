using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, Interactables
{
    public float MaxRange { get { return maxRange; } }

    [SerializeField] private float maxRange = 2f;

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
