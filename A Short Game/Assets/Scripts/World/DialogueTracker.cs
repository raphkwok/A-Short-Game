using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTracker : MonoBehaviour
{
    public bool flip;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 sp = Camera.main.WorldToScreenPoint(tracker.position);

        transform.LookAt(Camera.main.transform);
        if (!flip) transform.Rotate(0, 180, 0);
    }
    // void OnEnable()
    // {
    //     // this is here because there can be a single frame where the position is incorrect
    //     // when the object (or its parent) is activated.
    //     if (gameObject.activeInHierarchy)
    //     {
    //         Update();
    //     }
    // }
}
