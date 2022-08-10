using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingInput : MonoBehaviour
{
    //Rotation Variables
    [Header("Rotation Stats")]
    private float rotateInput;
    [SerializeField] private float rotationSpeed;
    private float rotation;


    //Casting Variables
    [Header("Casting Stats")]
    [SerializeField] private float sliderSpeed;
    [SerializeField] private Transform sliderValue;
    [SerializeField] private Transform sliderBase;
    private float sliderMax;
    private bool casting = false;
    private float power;
    //Slider Storage Variables
    private float maxLerp = 1f;
    private float minLerp = 0f;
    private float t = 0f;

    //Fishing Interactable Variables
    private float rotateMax;
    private float rotateMin;
    private float launchAngle;
    private float launchMultiplier;

    [Header("Prefabs")]
    [SerializeField] private GameObject fishingProjectile;


    public void OnRotate(InputValue value)
    {
        rotateInput = value.Get<float>();
    }

    public void OnCast(InputValue value)
    {
        power = 0;
        casting = true;
    }

    public void OnRelease(InputValue value)
    {
        GameObject projectile = Instantiate(fishingProjectile, transform.position, transform.rotation);
        projectile.GetComponent<FishingProjectile>().ReceiveValues(launchAngle, power, launchMultiplier);
    }

    public void ReceiveValues(float rotateMaxInteractable, float rotateMinInteractable, float launchAngleInteractable, float launchMultiplierInteractable)
    {
        rotateMax = rotateMaxInteractable;
        rotateMin = rotateMinInteractable;
        launchAngle = launchAngleInteractable;
        launchMultiplier = launchMultiplierInteractable;
    }

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        sliderMax = sliderBase.transform.localScale.z;
        sliderValue.transform.localScale = new Vector3(sliderValue.transform.localScale.x, sliderValue.transform.localScale.x, 0);
    }

    private void Update()
    {
        RotatePlayer();
        PowerChange();
    }

    private void RotatePlayer()
    {
        if (!casting)
        {
            rotation = Mathf.Clamp(rotation + rotateInput * rotationSpeed * Time.deltaTime, rotateMax, rotateMin);
            transform.eulerAngles = Vector3.up * rotation;
        }
    }

    private void PowerChange()
    {

        if (casting)
        {
            power = Mathf.Lerp(minLerp, maxLerp, t);

            t += Time.deltaTime * sliderSpeed;
            if (t >= 1f)
            {
                float temp = maxLerp;
                maxLerp = minLerp;
                minLerp = temp;
                t = 0f;
            }

            sliderValue.transform.localScale = new Vector3(sliderValue.transform.localScale.x, sliderValue.transform.localScale.x, power * sliderMax);
            sliderValue.transform.localPosition = Vector3.forward * sliderMax * power / 2;
            
        }
    }
}
