using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    [SerializeField] private float inverseMass;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 gravity = Vector3.zero;
    private Vector3 accumulatedForces = Vector3.zero;
    [SerializeField] private float dampingConstant;

    // getters and setters
    public float getRadius() { return transform.localScale.x * 0.5f; }
    public float getInverseMass() { return inverseMass; }
    public void setInverseMass(float m) { inverseMass = m; }
    public Vector3 getVelocity() { return velocity; }
    public void setVelocity(Vector3 vel) { velocity = vel; }
    public Vector3 getGravity() { return gravity; }
    public void setGravity(Vector3 g) { gravity = g; }
    public Vector3 getAccumulatedForces() { return accumulatedForces; }
    public void setAccumulatedForces(Vector3 f) { accumulatedForces = f; }
    public float getDampingConstant() { return dampingConstant; }
    public void setDampingConstant(float d) { dampingConstant = d; }

    private void Start()
    {
        // make sure static objects don't have a velocity
        if (inverseMass <= 0f)
        {
            velocity = Vector3.zero;
            gravity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        // exclude objects with infinite mass
        if (inverseMass <= 0f)
            return;

        velocity += gravity * Time.deltaTime;

        // update position and velocity
        Integrator.Integrate(this);
    }

    public void AddForce(Vector3 f)
    {
        accumulatedForces += f;
    }

    public void ClearAccumulatedForces()
    {
        accumulatedForces = Vector3.zero;
    }
}
