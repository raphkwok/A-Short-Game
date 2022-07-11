using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractCheck : MonoBehaviour
{

    private Interactables currentTarget;

    private Interactables interactable;

    public void OnInteract(InputValue value)
    {
        if (currentTarget != null)
        {
            currentTarget.OnInteract();
        }
    }

    void Update()
    {
        CheckCollider();
    }

    void CheckCollider()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        interactable = other.GetComponent<Interactables>();

        if (interactable != null)
        {
            if (currentTarget != null)
            {
                currentTarget.OnEndHover();
                currentTarget = interactable;
                currentTarget.OnStartHover();
            }
            else
            {
                currentTarget = interactable;
                currentTarget.OnStartHover();
            }
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentTarget != null)
        {
            currentTarget.OnEndHover();
            currentTarget = null;
        }
    }
}
