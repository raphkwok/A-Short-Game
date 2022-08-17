using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    [SerializeField] float moveSpeed;

    private Vector3 targetLocation;

    private BoxCollider bounds;

    public void ReceiveValues(BoxCollider areaBounds)
    {
        bounds = areaBounds;
        ChangeDirection();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetLocation, moveSpeed * Time.deltaTime);
        if (transform.position == targetLocation)
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
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

        targetLocation = transform.position + vector * distance;

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
