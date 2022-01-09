using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringForceGenerator : ForceGenerator
{

    [SerializeField] private Particle2D attachedParticle;
    [SerializeField] private float springConstant = 1f;
    [SerializeField] private float restLength = 1f;

    protected override void UpdateForce(Particle3D particle)
    {
        if (attachedParticle) // ensure the other particle still exists
        {
            // calculate vector of the spring
            Vector3 forceVector = transform.position - attachedParticle.gameObject.transform.position;

            // calcuulate magnitude of the force
            float magnitude = (forceVector.magnitude - restLength) * -springConstant;

            // finalize force and apply it
            forceVector = forceVector.normalized * magnitude;
            particle.AddForce(forceVector);
            attachedParticle.AddForce(-forceVector);
        }
    }

    public void SetAttachedParticle(Particle2D part)
    {
        attachedParticle = part;
    }

}
