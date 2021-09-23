using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceType : MonoBehaviour
{   
    public ParticleSystem arrivalDust;

    void Start()
    {
        arrivalDust = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        
    }
}
