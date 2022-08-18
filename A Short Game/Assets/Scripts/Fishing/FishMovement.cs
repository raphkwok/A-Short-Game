using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    [SerializeField] float moveSpeed;

    private Vector3 lastLocation;
    private Vector3 targetLocation;
    private bool pickingNewLocation = false;
    private bool started = false;

    private BoxCollider bounds;

    public void ReceiveValues(BoxCollider areaBounds)
    {
        bounds = areaBounds;
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        if (started == true)
        {
            Move();
            CheckDistance();
        }

    }

    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetLocation, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetLocation) <= 1 && pickingNewLocation == false)
        {
            StartCoroutine(ChangeDirection());
            pickingNewLocation = true;
        }
    }
    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, lastLocation) > 1)
        {
            pickingNewLocation = false;
        }
    }
    //coroutines def fucking with the movement and location selection but like it looks fine yk

    IEnumerator ChangeDirection()
    {
        print("start");
        yield return new WaitForSeconds(Random.Range(0, 2));
        Vector3 boxPoint = GetRandomPointInsideCollider(bounds);

        Vector3 vector = Vector3.Normalize(boxPoint - transform.position);

        float distance;

        if (Vector3.Distance(boxPoint, transform.position) < minDistance)
        {
            distance = Vector3.Distance(boxPoint, transform.position);
        }
        else if (Vector3.Distance(boxPoint, transform.position) > maxDistance)
        {
            distance = Random.Range(minDistance, maxDistance);
        }
        else
        {
            distance = Random.Range(minDistance, Vector3.Distance(boxPoint, transform.position));
        }
        lastLocation = targetLocation;
        targetLocation = transform.position + vector * distance;
        print(targetLocation);
        started = true;
        print("end");
    }


    public Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size * 1.4f / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + boxCollider.center;

        return boxCollider.transform.TransformPoint(point);
    }
}
