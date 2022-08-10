using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private float launchAngle;
    private float power;
    private float launchMultiplier;

    public void ReceiveValues(float launchAngleInput, float powerInput, float launchMultiplierInput)
    {
        launchAngle = launchAngleInput;
        power = powerInput;
        launchMultiplier = launchMultiplierInput;
        Fire();
    }

    private void Fire()
    {
        rb.AddForce((new Vector3(transform.forward.x, Mathf.Tan(Mathf.Deg2Rad * launchAngle), transform.forward.z)) * launchMultiplier * power, ForceMode.Impulse);
        print(transform.forward);
    }
}