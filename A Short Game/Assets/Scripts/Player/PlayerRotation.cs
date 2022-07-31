using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    private Vector2 mouseInput;

    public void OnLook(InputValue value)
    {
        mouseInput = value.Get<Vector2>();
    }

    private void Awake()
    {
        SetVar();
    }

    void SetVar()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        CamRotation();
    }

    void CamRotation()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + mouseInput.x, transform.eulerAngles.z);
    }
}
