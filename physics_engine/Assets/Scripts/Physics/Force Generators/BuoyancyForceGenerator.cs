using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyForceGenerator : ForceGenerator
{
    [SerializeField] private float liquidDensity = 100f;
    [SerializeField] private float volume;
    private float maxDepth; // stores how far underwater the object needs to be in order to be considered fully submerged
    private WaterManager water;

    private void Start()
    {
        base.Start();

        water = WaterManager.GetInstance();

        // assign variables needed for buoyancy calculation
        maxDepth = transform.localScale.y * 0.5f;

        if (volume == 0f)
        {
            float radius = transform.localScale.y * 0.5f; // assumed to be spherical - will approximately work with boxes and other shapes
            volume = 4.189f * radius * radius * radius;
        }
    }
    
    // buoyancy equations derived from Millington - Game Physics Engine Development chapter 6.2
    protected override void UpdateForce(Particle3D particle)
    {
        // determine how far underwater we are
        float depth = water.getWaterLevel(particle.transform.position) - particle.transform.position.y;

        // check if we are completely above the water
        if (depth < -maxDepth)
            return;

        // init force
        Vector3 force = new Vector3(0f, 0f, 0f);

        // check if we are completely below the water
        if (depth > maxDepth)
        {
            force.y = liquidDensity * volume;
            particle.AddForce(force);
            return;
        }

        // otherwise we are partially submerged
        float d = particle.transform.position.y;
        float w = water.getWaterLevel(particle.transform.position);
        force.y = liquidDensity * volume * (d - maxDepth - w) / -2f * maxDepth;
        particle.AddForce(force);
    }

}
