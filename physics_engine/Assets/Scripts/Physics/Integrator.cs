using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    public static void Integrate(Particle2D particle)
    {
        // update velocity based on accumulated forces
        Vector2 resultingAcc = particle.getAccumulatedForces() * particle.getInverseMass();     // scale accumulated forces by inverse mass. a = f/m
        particle.setVelocity(particle.getVelocity() + resultingAcc * Time.deltaTime);                 // apply to velocity

        // update position based on velocity
        Vector2 newPos = particle.getVelocity() * Time.deltaTime;
        particle.transform.position += new Vector3(newPos.x, newPos.y, 0f); // lock z-axis to zero

        // impose drag
        // velocity *= damping^time
        particle.setVelocity(particle.getVelocity() * Mathf.Pow(particle.getDampingConstant(), Time.deltaTime));

        // clear forces from the accumulator
        particle.ClearAccumulatedForces();
    }

    public static void Integrate(Particle3D particle)
    {
        // update velocity based on accumulated forces
        Vector3 resultingAcc = particle.getAccumulatedForces() * particle.getInverseMass();     // scale accumulated forces by inverse mass. a = f/m
        particle.setVelocity(particle.getVelocity() + resultingAcc * Time.deltaTime);                 // apply to velocity

        // update position based on velocity
        Vector3 newPos = particle.getVelocity() * Time.deltaTime;
        particle.transform.position += new Vector3(newPos.x, newPos.y, newPos.z);

        // impose drag
        // velocity *= damping^time
        particle.setVelocity(particle.getVelocity() * Mathf.Pow(particle.getDampingConstant(), Time.deltaTime));

        // clear forces from the accumulator
        particle.ClearAccumulatedForces();
    }
}
