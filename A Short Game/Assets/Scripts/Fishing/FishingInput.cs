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
    [SerializeField] private float recallSpeed = 20;
    private float sliderMax;  
    private float power;

    //Storage for Line End
    private GameObject projectile;
    private GameObject caughtFish;
    private string caughtFishName;

    //States
    private bool casting = false;
    private bool projectileOut = false;
    private bool recalling = false;

    //Slider Storage Variables
    private float maxLerp = 1f;
    private float minLerp = 0f;
    private float t = 0f;

    //Fishing Interactable Variables
    private float initialRotate;
    private float rotateMax;
    private float rotateMin;
        //Projectile Variables
    private float launchAngle;
    private float launchMultiplier;
    private float areaWidth;
    private float areaLength;

    [Header("Prefabs")]
    [SerializeField] private GameObject fishingProjectile;


    public void OnRotate(InputValue value)
    {
        rotateInput = value.Get<float>();
    }

    public void OnCast(InputValue value)
    {
        if (!recalling && !projectileOut)
        {
            power = 0;
            casting = true;
        }
    }

    public void OnRelease(InputValue value)
    {
        if (casting)
        {
            projectile = Instantiate(fishingProjectile, transform.position, transform.rotation);
            projectile.GetComponent<FishingProjectile>().ReceiveValues(launchAngle, power, launchMultiplier, areaWidth, areaLength);
            power = 0;
            casting = false;
            projectileOut = true;
        }
    }

    public void OnRecall(InputValue value)
    {
        if (projectileOut)
        {
            projectileOut = false;
            recalling = true;
            if (projectile.GetComponent<FishingProjectile>().currentFish != null)
            {
                caughtFish = projectile.GetComponent<FishingProjectile>().currentFish;
                caughtFishName = caughtFish.name;
                Destroy(caughtFish);
            }
        }
    }

    public void ReceiveValues(float rotateMaxInteractable, float rotateMinInteractable, float launchAngleInteractable, float launchMultiplierInteractable, float areaWidthInteractable, float areaLengthInteractable)
    {
        rotateMax = rotateMaxInteractable;
        rotateMin = rotateMinInteractable;
        launchAngle = launchAngleInteractable;
        launchMultiplier = launchMultiplierInteractable;
        areaWidth = areaWidthInteractable;
        areaLength = areaLengthInteractable;
    }

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        sliderMax = sliderBase.transform.localScale.z;
        sliderValue.transform.localScale = new Vector3(sliderValue.transform.localScale.x, sliderValue.transform.localScale.x, 0);
        initialRotate = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        if (!casting && !projectileOut && !recalling)
        {
            RotatePlayer();
        }
        if (casting)
        {
            PowerChange();
        }
        if (recalling)
        {
            RecallProjectile();
            CheckRecallProximity();
        }
        SetSlider();
    }

    private void RotatePlayer()
    {
        rotation = Mathf.Clamp(rotation + rotateInput * rotationSpeed * Time.deltaTime, rotateMax, rotateMin);
        transform.eulerAngles = Vector3.up * (rotation + initialRotate);
    }

    private void PowerChange()
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
    }

    private void SetSlider()
    {
        sliderValue.transform.localScale = new Vector3(sliderValue.transform.localScale.x, sliderValue.transform.localScale.x, power * sliderMax);
        sliderValue.transform.localPosition = Vector3.forward * sliderMax * power / 2;
    }

    private void RecallProjectile()
    {
        projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, transform.position, Time.deltaTime * recallSpeed);
    }

    private void CheckRecallProximity()
    {
        if (Vector3.Distance(projectile.transform.position, transform.position) <= 0)
        {
            Destroy(projectile);
            recalling = false;
        }
    }
}
