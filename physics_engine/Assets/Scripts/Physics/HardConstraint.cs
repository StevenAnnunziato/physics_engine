using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class acts as a metal rod connecting two particles.
public class HardConstraint : MonoBehaviour
{

    [SerializeField] private Particle3D other;
    private Particle3D myParticle;
    private float length;
    private CollisionManager collisionManager;

    // Start is called before the first frame update
    void Start()
    {
        myParticle = GetComponent<Particle3D>();
        collisionManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CollisionManager>();

        // determine distance to enforce between the objects
        length = Vector3.Distance(other.gameObject.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float currentLength = Vector3.Distance(other.gameObject.transform.position, transform.position);

        // exit if possible
        if (currentLength == length)
            return;

        // send a fake collision to the collision manager to enforce proper length
        float penetration;
        Vector3 contactNormal = (other.transform.position - myParticle.transform.position).normalized;
        // contact normal depends on whether we're overextended or underextended
        if (currentLength > length)
        {
            penetration = currentLength - length;
        }
        else
        {
            contactNormal *= -1;
            penetration = length - currentLength;
        }

        // account for the scale of the objects
        penetration += other.getRadius() + myParticle.getRadius();

        collisionManager.CollideSpheres(other, myParticle, penetration, contactNormal, 1f);
    }
}
