using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    Vector2 mouseInput;

    //Cam rotations
    private float yaw;
    [SerializeField] private float sens = 0f;
    [SerializeField] private float smoothAmount = 0f;


    public void OnLook(InputValue value)
    {
        mouseInput = value.Get<Vector2>();
    }

    private void Awake()
    {
        InitValues();
        CursorSettings();
    }

    void InitValues()
    {
        yaw = transform.eulerAngles.y;
    }

    void CursorSettings()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CalculateRotation();
        ApplyRotation();
    }

    void CalculateRotation()
    {
        yaw += mouseInput.x * sens * Time.deltaTime;
    }

    void ApplyRotation()
    {
        transform.eulerAngles = new Vector3(0f, yaw, 0f);
    }
}
