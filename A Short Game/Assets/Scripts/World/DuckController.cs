using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    // [Header("State Settings")] 
    public enum State { Swimming, Walking };
    public State state;

    [Header("Movement Speed")]
    public float movementSpeed;
    public float rotationSpeed;
    public float swimSpeed;

    [Header("Rotation Settings")]
    public float maxRotation;
    public float angle;
    public float randomRotationTime;

    [Header("Sensory Settings")]
    public float duckRadius;
    public float wallDetectionDist;


    // Start is called before the first frame update
    void Start()
    {
        NextState();
    }

    // Update is called once per frame
    void Update()
    {
        // print(Physics.Raycast(transform.position, transform.forward, wallDetectionDist));
    }

    ////////// Functions //////////
    public void ChangeRotation()
    {
        angle = Mathf.Repeat(Random.Range(0, maxRotation) + angle - maxRotation / 2, 360);
    }

    ////////// Coroutines //////////
    IEnumerator RotationTimer()
    {
        yield return null;
    }

    ////////// STATES //////////

    public void NextState()
    {
        string methodName = state.ToString() + "State";

        // Get method
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);

        StartCoroutine((IEnumerator)info.Invoke(this, null)); // Call the next state
    }

    IEnumerator SwimmingState()
    {
        float timer = 0;
        while (state == State.Swimming)
        {
            // Move forwards in direction d
            transform.Translate(Vector3.forward * swimSpeed * Time.deltaTime);

            // Rotate towards direction

            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, angle, rotationSpeed * Time.deltaTime), 0);

            // Check for other ducks, if behind, slow down

            // Check for wall in front, if too close, change direction, also change direction if timer is up
            if (Physics.Raycast(transform.position, transform.forward, wallDetectionDist) || timer >= randomRotationTime)
            {
                ChangeRotation();
                timer = 0;
            }

            timer += Time.deltaTime;
            // Check for walls around, if too close, change direction
            yield return null;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * wallDetectionDist);
    }
    IEnumerator Walking()
    {
        yield return null;
    }
}