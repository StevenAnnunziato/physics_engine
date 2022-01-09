using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionForceGenerator : ForceGenerator
{
    [SerializeField] private float forceIntensity = 1f;
    private int forceDirection;
    private Vector3 mousePosition;
    private Camera cam;

    private void Start()
    {
        base.Start();

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();

        // get mouse position in world coordinates
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // lock z-axis

        // get input to determine force direction
        if (Input.GetMouseButton(0))
            forceDirection = 1;
        else if (Input.GetMouseButton(1))
            forceDirection = -1;
        else
            forceDirection = 0;
    }

    protected override void UpdateForce(Particle3D particle)
    {
        // get force direction
        Vector3 force = mousePosition - transform.position;
        force.Normalize();

        // weaken force by a factor of distance^2
        float dist = force.magnitude;
        force /= dist * dist;

        // apply desired direction based on mouse input
        force *= forceDirection;

        // apply to particle
        particle.AddForce(force * forceIntensity);

    }
}
