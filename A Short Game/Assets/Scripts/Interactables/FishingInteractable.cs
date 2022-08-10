using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FishingInteractable : MonoBehaviour, Interactables
{


    private GameObject player;

    [Header("Prefabs")]
    [SerializeField] private GameObject fishingPlayer;
    [SerializeField] private CinemachineVirtualCamera fishingCam;


    [Header("Fishing Spot Settings")]
    [SerializeField] private float rotationMax;
    [SerializeField] private float rotationMin;
    [SerializeField] private bool invertPos;
    [SerializeField] private float areaWidth;
    [SerializeField] private float areaLength;
    [SerializeField] private float launchAngle;
    [SerializeField] private float launchMultiplier;

    private float posVariable;

    private void Awake()
    {
        if (invertPos)
        {
            posVariable = -1f;
        }
        else
        {
            posVariable = 1f;
        }
    }

    public void OnStartHover()
    {
        print("start");
    }

    public void OnInteract()
    {
        player = GameObject.Find("Player");
        player.SetActive(false);
        GameObject fishingObject = Instantiate(fishingPlayer, transform.position, Quaternion.identity);
        fishingObject.GetComponent<FishingInput>().ReceiveValues(-rotationMax, rotationMin, launchAngle, launchMultiplier);
        CinemachineVirtualCamera cam = Instantiate(fishingCam, transform.position + new Vector3(areaWidth * posVariable, 10, areaLength / 2), Quaternion.identity);
        cam.GetComponent<FishingCamControl>().ReceiveValues(areaLength);
    }

    public void OnEndHover()
    {
        print("end");
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, new Vector3(Mathf.Tan(Mathf.Deg2Rad * rotationMin), 0, 1) * 30);
        Gizmos.DrawLine(transform.position, new Vector3(- Mathf.Tan(Mathf.Deg2Rad * rotationMax), 0, 1) * 30);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, Mathf.Tan(Mathf.Deg2Rad * launchAngle), 1) * launchMultiplier);
        Gizmos.DrawWireCube(transform.position + Vector3.forward * areaLength / 2, new Vector3(areaWidth, 0, areaLength));
    }
}
