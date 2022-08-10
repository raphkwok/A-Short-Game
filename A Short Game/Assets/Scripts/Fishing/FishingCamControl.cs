using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FishingCamControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;

    public void ReceiveValues(float areaLength)
    {
        cam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = Vector3.forward * areaLength / 2;
    }

    private void Awake()
    {
        cam.Priority = 20;
        cam.m_LookAt = GameObject.Find("Fishing Player(Clone)").GetComponent<Transform>();
    }
}
