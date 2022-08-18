using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float floatStrength = 0.01f;
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float fishingLineWidth = 1;
    [SerializeField] private float fishingLineDepth = Mathf.Infinity;
    [SerializeField] private LayerMask detectLayers;
    private float launchAngle;
    private float power;
    private float launchMultiplier;
    public GameObject currentFish = null;

    private bool hitWater = false;

    public void ReceiveValues(float launchAngleInput, float powerInput, float launchMultiplierInput, float areaWidthInput, float areaLengthInput)
    {
        launchAngle = launchAngleInput;
        power = powerInput;
        launchMultiplier = launchMultiplierInput;
        Fire();
    }

    private void Fire()
    {

        rb.AddForce((new Vector3(transform.forward.x, Mathf.Tan(Mathf.Deg2Rad * launchAngle), transform.forward.z)) * launchMultiplier * power, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {

        if (hitWater)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            /**
            float bobValue = Time.timeSinceLevelLoad / floatSpeed;

            float distance = floatStrength * (Mathf.Sin(bobValue) + 1);
            //print(distance);
            transform.position = new Vector3(hitPos.x, hitPos.y + distance, hitPos.z);
            **/
        }

        CheckFish();
        //print(currentFish);
    }

    void CheckFish()
    {
        RaycastHit hit;

        /**
        if (Physics.BoxCast(transform.position + Vector3.down * fishingLineDepth / 2, new Vector3(fishingLineWidth, fishingLineDepth, fishingLineWidth), - transform.up, out hit, transform.rotation, detectLayers))
        {
            Physics.Raycast(transform.position, Vector3.down, out hit, fishingLineDepth, detectLayers)
        }
        **/

        if (Physics.BoxCast(transform.position + Vector3.down * fishingLineDepth / 2, new Vector3(fishingLineWidth, fishingLineDepth, fishingLineWidth), -transform.up, out hit, transform.rotation, detectLayers))
        {
            if (currentFish = hit.transform.gameObject)
            {
                return;
            }
            else if (hit.transform.gameObject.tag == "Fish")
            {
                currentFish = hit.transform.gameObject;
                return;
            }
            else
            {
                currentFish = null;
                return;
            }
        }
        else
        {
            currentFish = null;
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hitWater)
        {
            hitWater = true;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + Vector3.down * fishingLineDepth / 2, new Vector3(fishingLineWidth, fishingLineDepth, fishingLineWidth));
    }
}