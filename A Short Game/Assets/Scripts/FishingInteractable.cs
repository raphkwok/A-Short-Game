using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingInteractable : MonoBehaviour, Interactables
{
    public GameObject player;


    public void OnStartHover()
    {
        print("Fishing Text");
    }

    public void OnInteract()
    {
        LockPlayer();
        GetComponent<Collider>().enabled = false;
    }

    public void OnEndHover()
    {
        UnlockPlayer();
    }



    void LockPlayer()
    {
        player.GetComponent<Movement>().enabled = false;
        player.transform.position = transform.position;
    }

    void UnlockPlayer()
    {
        player.GetComponent<Movement>().enabled = true;
    }

}
