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
    [SerializeField] private LayerMask playerLayer;
    private Interactables currentTarget;
    private Camera mainCamera;


    private void Awake()
    {
        mainCamera = Camera.main;
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

        Collider[] hit = Physics.OverlapSphere(transform.position + transform.forward * range, radius, playerLayer);



        if (hit.Length > 0)
        {
            //print("hit");

            //var colliderObject = hit.

            GameObject colliderObject = hit[0].gameObject;

            Interactables interactable = colliderObject.GetComponent<Interactables>();

  

            if (interactable != null)
            {
                if (Vector3.Distance(transform.position, colliderObject.transform.position) <= interactable.MaxRange)
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

