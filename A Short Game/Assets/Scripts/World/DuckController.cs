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

    IEnumerator Swimming()
    {
        while (state == State.Swimming)
        {
            // Move forwards in direction d

            // Rotate towards direction
            angle = Mathf.LerpAngle(angle, transform.eulerAngles.y, rotationSpeed * Time.deltaTime);

            // Check for other ducks, if behind, slow down

            // Check for wall in front, if too close, change direction
            if (Physics.Raycast(transform.position, transform.forward, wallDetectionDist))
            {
                ChangeRotation();
            }

            // Check for walls around, if too close, change direction
            yield return null;
        }


    }

    IEnumerator Walking()
    {
        yield return null;
    }
}