using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    [SerializeField] private float inverseMass;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 gravity = Vector2.zero;
    private Vector2 accumulatedForces = Vector2.zero;
    [SerializeField] private float dampingConstant;

    // getters and setters
    public float getInverseMass() { return inverseMass; }
    public void setInverseMass(float m) { inverseMass = m; }
    public Vector2 getVelocity() { return velocity; }
    public void setVelocity(Vector2 vel) { velocity = vel; }
    public Vector2 getGravity() { return gravity; }
    public void setGravity(Vector2 g) { gravity = g; }
    public Vector2 getAccumulatedForces() { return accumulatedForces; }
    public void setAccumulatedForces(Vector2 f) { accumulatedForces = f; }
    public float getDampingConstant() { return dampingConstant; }
    public void setDampingConstant(float d) { dampingConstant = d; }

    private void FixedUpdate()
    {
        // exclude objects with infinite mass
        if (inverseMass <= 0f)
            return;

        velocity += gravity * Time.deltaTime;

        // update position and velocity
        Integrator.Integrate(this);
    }

    public void AddForce(Vector2 f)
    {
        accumulatedForces += f;
    }

    public void ClearAccumulatedForces()
    {
        accumulatedForces = Vector2.zero;
    }
}
