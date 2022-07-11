using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Transform aimBox;
    private Vector3 aimBoxCoords;
    private Vector3 aimBoxInitial;
    public LayerMask aimLayer;
    public GameObject slider;
    public GameObject indicator;

    public GameObject hook;

    private Vector2 mousePos;
    private Vector3 localHit;

    [SerializeField] private float minROM = 20f;
    [SerializeField] private float maxROM = 60f;
    [SerializeField] private float turnSmoothAmount = 0f;
    [SerializeField] private float sliderSmoothAmount = 0f;
    [SerializeField] private float maxPull;
    [SerializeField] private float sliderLength;

    [SerializeField] private Vector3 throwDirection;
    [SerializeField] private float throwStrength;


    private float angle;
    private float desiredAngle;
    private float distance;
    private float desiredDistance;

    private bool pulling;

    private void Awake()
    {
        aimBoxCoords = aimBox.transform.position;
        aimBoxInitial = aimBox.transform.localScale;
        hook.SetActive(false);
    }

    public void OnAim(InputValue value)
    {
        mousePos = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        aimBox.transform.position = new Vector3(aimBoxCoords.x + localHit.x, aimBoxCoords.y, aimBoxCoords.z + localHit.z);
        aimBox.transform.localScale = aimBox.transform.localScale * 2;
        indicator.SetActive(true);
        slider.SetActive(true);
        pulling = true;
    }

    public void OnRelease(InputValue value)
    {
        hook.SetActive(true);
        hook.GetComponent<Transform>().eulerAngles = Vector3.up * angle;
        //hook.GetComponent<Rigidbody>().velocity = throwDirection * throwStrength;
        hook.GetComponent<Rigidbody>().AddForce((throwDirection - localHit) * throwStrength);
        //still have to take angle not local hit bc rn can shoot backwards

        pulling = false;
    }

    private void Update()
    {
        MouseInput();
        SmoothIndicator();
        IndicatorLength();
        SmoothRotation();
        ApplyRotation();
        ResetObjects();
    }

    void MouseInput()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, Mathf.Infinity, aimLayer))
        {
            localHit = aimBox.InverseTransformPoint(hit.point);

            // May have to change depending on where player but will deal with that in the game scene

            // if it works it works
            if (localHit != Vector3.zero)
            {
                desiredAngle = ((Mathf.Atan2(localHit.z, localHit.x) * Mathf.Rad2Deg) + 360) % 360;
                print(localHit);
            }

            if (pulling)
            {
                desiredDistance = Mathf.Clamp(Vector3.Distance(localHit, Vector3.zero), 0, maxPull);
            }
            else
            {
                desiredDistance = 0;
            }

            desiredAngle = Mathf.Clamp(desiredAngle, 180 - minROM, 180 + maxROM);
            desiredAngle = -desiredAngle + 180;
        } 
    }

    void SmoothIndicator()
    {
        distance = Mathf.MoveTowards(distance, desiredDistance, Time.deltaTime * sliderSmoothAmount);
    }

    void IndicatorLength()
    {
        slider.transform.localScale = new Vector3(distance / maxPull * sliderLength, slider.transform.localScale.y, slider.transform.localScale.y);
    }

    void SmoothRotation()
    {
        angle = Mathf.LerpAngle(angle, desiredAngle, Time.deltaTime * turnSmoothAmount);
    }

    void ApplyRotation()
    {
        transform.eulerAngles = Vector3.up * angle;
        
    }

    void ResetObjects()
    {
        if (!pulling && distance == 0)
        {
            aimBox.transform.position = aimBoxCoords;
            aimBox.transform.localScale = aimBoxInitial;
            indicator.SetActive(false);
            slider.SetActive(false);
        }
    }
}
