using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    public Vector3 magnitude = Vector3.one;
    public float timeScale = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (timeScale == 0)
        {
            timeScale = Random.Range(0.6f, 2.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.localEulerAngles;

        rotation.x = magnitude.x * Mathf.Sin(timeScale * Time.time);
        rotation.y = magnitude.y * Mathf.Sin(timeScale * Time.time);
        rotation.z = magnitude.z * Mathf.Sin(timeScale * Time.time);

        transform.localEulerAngles = rotation;
    }
}
