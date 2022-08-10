using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRaycast : MonoBehaviour
{

    [SerializeField] private float range;
    private Vector3 distance;
    [SerializeField] private float width;
    [SerializeField] private float length;
    [SerializeField] private float radius;
    private Interactables currentTarget;



    private void Awake()
    {
        distance = (transform.localPosition + transform.forward * range);
    }

    public void OnInteract(InputValue value)
    {
        if (currentTarget != null)
        {
            currentTarget.OnInteract();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * range, radius);
    }

    private void Update()
    {
        RaycastForInteractables();

    }

    private void RaycastForInteractables()
    {
        //RaycastHit hit;

        Collider[] hit = Physics.OverlapSphere(transform.position + transform.forward * range, radius);

        Interactables interactable = null;


        if (hit.Length > 0)
        {

            foreach (var collider in hit)
            {
                Interactables temp;
                temp = collider.GetComponent<Interactables>();

                if (temp != null)
                {
                    interactable = temp;
                }
            }

            if (interactable != null)
            {
                if (interactable == currentTarget)
                {
                    return;
                }
                else if (currentTarget != null)
                {
                    currentTarget.OnEndHover();
                    currentTarget = interactable;
                    currentTarget.OnStartHover();
                    return;
                }
                else
                {
                    currentTarget = interactable;
                    currentTarget.OnStartHover();
                    return;
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.OnEndHover();
                    currentTarget = null;
                    return;
                }
            }
        }
        else
        {
            if (currentTarget != null)
            {
                currentTarget.OnEndHover();
                currentTarget = null;
                return;
            }
        }
    }
}

