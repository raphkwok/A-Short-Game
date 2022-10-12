using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem particle;
    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        if (particle)
        {
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }
    }

    void OnDisable()
    {
        if (particle)
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }
    }
}
