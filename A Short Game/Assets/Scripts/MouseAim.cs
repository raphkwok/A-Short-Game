using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class MouseAim : MonoBehaviour
{
    public Transform aimBox;
    private Vector3 aimBoxCoords;
    public LayerMask aimLayer;
    public Transform slider;

    private Vector2 mousePos;
    private Vector3 localHit;
    private Vector3 initialPos;

    [SerializeField] private float minROM = 20;
    [SerializeField] private float maxROM = 60;


    private float angle;
    private float distance;


    private void Awake()
    {
        aimBoxCoords = aimBox.transform.position;
    }

    public void OnAim(InputValue value)
    {
        mousePos = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        aimBox.transform.position = new Vector3(aimBoxCoords.x + localHit.x, aimBoxCoords.y, aimBoxCoords.z + localHit.z);
        aimBox.transform.localScale = aimBox.transform.localScale * 2;

    }

    public void OnRelease(InputValue value)
    {
        aimBox.transform.position = aimBoxCoords;
        aimBox.transform.localScale = aimBox.transform.localScale / 2;
    }

    private void Update()
    {
        MouseInput();
        ApplyRotation();
        PullBack();
    }

    void MouseInput()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, Mathf.Infinity, aimLayer))
        {
            localHit = aimBox.InverseTransformPoint(hit.point);
            //print(localHit);

            // May have to change depending on where player but will deal with that in the game scene
            angle = -(Mathf.Atan2(aimBox.transform.localPosition.z + localHit.z, aimBox.transform.localPosition.x + localHit.x) * Mathf.Rad2Deg - 180);
            
            distance = Vector3.Distance(localHit, Vector3.zero);
        } 
    }

    void ApplyRotation()
    {
        // couldnt clamp the shit so had to do this L
        if ((angle > (360 - maxROM)) || angle < minROM)
        {
            transform.eulerAngles = Vector3.up * angle;
        }
    }

    void PullBack()
    {

    }
}
