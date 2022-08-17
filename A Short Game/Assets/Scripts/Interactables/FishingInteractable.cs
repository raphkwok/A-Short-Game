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
    [SerializeField] private GameObject fishingBounds;
    [SerializeField] private GameObject fishSpawner;


    [Header("Fishing Spot Settings")]
    [SerializeField] private float rotationMax;
    [SerializeField] private float rotationMin;
    [SerializeField] private bool invertPos;
    [SerializeField] private float areaWidth;
    [SerializeField] private float areaLength;
    [SerializeField] private float launchAngle;
    [SerializeField] private float launchMultiplier;

    [Header("Border")]
    [SerializeField] private Material borderMat;

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
        DrawLine();
        player = GameObject.Find("Player");
        player.SetActive(false);
        GameObject fishingObject = Instantiate(fishingPlayer, transform.position, transform.rotation);
        fishingObject.GetComponent<FishingInput>().ReceiveValues(-rotationMax, rotationMin, launchAngle, launchMultiplier, areaWidth, areaLength);

        //Will play around with this to get the right adaptive values
        CinemachineVirtualCamera cam = Instantiate(fishingCam, transform.position + transform.right * areaWidth * posVariable * 1.2f + transform.up * 10 + transform.forward * areaLength / 2, transform.rotation);
        cam.GetComponent<FishingCamControl>().ReceiveValues(areaLength);

        GameObject areaBounds = Instantiate(fishingBounds, transform.position, transform.rotation);
        areaBounds.GetComponent<FishingBounds>().ReceiveValues(areaLength, areaWidth);

        GameObject spawner = Instantiate(fishSpawner, transform.position, transform.rotation);
        spawner.GetComponent<FishSpawner>().ReceiveValues(areaWidth, areaLength);
    }

    public void OnEndHover()
    {
        print("end");
    }

    void DrawLine()
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = transform.localPosition;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = borderMat;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.positionCount = 4;
        lr.SetPosition(0, transform.position + transform.right * areaWidth / 2);
        lr.SetPosition(1, transform.position - transform.right * areaWidth / 2);
        lr.SetPosition(3, transform.position + transform.right * areaWidth / 2 + transform.forward * areaLength);
        lr.SetPosition(2, transform.position - transform.right * areaWidth / 2 + transform.forward * areaLength);
        lr.loop = true;
    }

    void Update()
    {
        //print(transform.position + transform.right * areaWidth * posVariable * 1.2f + transform.up * 10 + transform.forward * areaLength / 2);
        //print(transform.position + transform.right * areaWidth * posVariable * 1.2f);
        //print(transform.position + transform.forward * areaLength / 2);
    }



    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        //Rotation Limits
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(Vector3.up * rotationMin) * transform.forward * 30);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(Vector3.up * - rotationMax) * transform.forward * 30);

        //Launch Angle
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(Vector3.right * - launchAngle) * transform.forward * launchMultiplier);

        //Area Bounds
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.forward * areaLength / 2, new Vector3(areaWidth, 0, areaLength));
    }
}
