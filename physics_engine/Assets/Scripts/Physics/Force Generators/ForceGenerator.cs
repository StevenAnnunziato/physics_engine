using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator : MonoBehaviour
{

    private Particle3D myParticle;

    protected void Start()
    {
        myParticle = GetComponent<Particle3D>();
    }

    protected void FixedUpdate()
    {
        UpdateForce(myParticle);
    }

    protected virtual void UpdateForce(Particle3D particle)
    {
        // particle.AddForce(something);
    }

}
