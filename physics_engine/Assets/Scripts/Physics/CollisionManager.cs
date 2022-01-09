using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    private GameObject[] particles;
    [SerializeField] private float restitutionConstant = 1f;

    class Plane
    {
        public Vector3 normal;
        public float originOffset;
        public Particle3D particle;

        // constructor
        public Plane(Vector3 n, float l)
        {
            normal = n;
            originOffset = l;
            particle = new Particle3D();

            // make the plane have infinite mass since it will not move
            particle.setInverseMass(0f);
        }
    }

    private Plane[] collisionPlanes;

    // Start is called before the first frame update
    void Start()
    {
        // place four planes on the edges of the screen
        collisionPlanes = new Plane[1];

        // construct planes
        collisionPlanes[0] = new Plane(Vector3.up, -100); // bottom
        /*collisionPlanes[1] = new Plane(Vector3.left, -5); // right
        collisionPlanes[2] = new Plane(Vector3.down, -5); // top
        collisionPlanes[3] = new Plane(Vector3.right, -5); // left
        collisionPlanes[4] = new Plane(Vector3.forward, -5); // back
        collisionPlanes[5] = new Plane(Vector3.back, -5);*/ // front
    }

    // Update is called once per frame
    void Update()
    {
        particles = GameObject.FindGameObjectsWithTag("Projectile");

        for (int i = 0; i < particles.Length - 1; i++)
        {
            for (int j = i + 1; j < particles.Length; j++)
            {
                // check collisions between i and j
                CheckParticleCollisions(particles[i].GetComponent<Particle3D>(), particles[j].GetComponent<Particle3D>());
            }

            // check for plane collisions
            CheckPlaneCollisions(particles[i].GetComponent<Particle3D>(), collisionPlanes);

            // edge case for the last element of the array
            if (i == particles.Length - 2)
            {
                CheckPlaneCollisions(particles[i + 1].GetComponent<Particle3D>(), collisionPlanes);
            }
        }

        // edge case for if there is only one particle
        if (particles.Length == 1)
        {
            CheckPlaneCollisions(particles[0].GetComponent<Particle3D>(), collisionPlanes);
        }
    }

    // Determines if two particles are colliding.
    void CheckParticleCollisions(Particle3D a, Particle3D b)
    {
        AABB aBox = a.gameObject.GetComponent<AABB>();
        AABB bBox = b.gameObject.GetComponent<AABB>();

        // two spheres
        if (aBox == null && bBox == null)
        {
            // check for a collision
            float midline = Vector3.Distance(a.transform.position, b.transform.position);
            float maxDist = a.transform.localScale.x * 0.5f + b.transform.localScale.x * 0.5f;
            if (midline < maxDist)
            {
                // collision
                Vector3 contactNormal = (a.transform.position - b.transform.position).normalized;
                ResolveSphereInterpenetration(a, b, midline, contactNormal);
                ResolveVelocity(a, b, contactNormal, restitutionConstant);
            }
        }

        // two boxes
        else if (aBox != null && bBox != null)
        {
            // check if a hit occurs on any axis
            if (Mathf.Abs(a.transform.position.x - b.transform.position.x) < a.transform.localScale.x * 0.5f + b.transform.localScale.x * 0.5 &&
                Mathf.Abs(a.transform.position.y - b.transform.position.y) < a.transform.localScale.y * 0.5f + b.transform.localScale.y * 0.5 &&
                Mathf.Abs(a.transform.position.z - b.transform.position.z) < a.transform.localScale.z * 0.5f + b.transform.localScale.z * 0.5)
            {
                Vector3 contactNormal = Vector3.zero;
                ResolveBoxInterpenetration(a, b, out contactNormal);
                ResolveVelocity(a, b, contactNormal, restitutionConstant);
            }
            
        }

        // A is a sphere, B is a box
        else if (aBox == null && bBox != null)
        {
            // determine closest point from the sphere to the box
            Vector3 closestPoint = a.transform.position;
            // clamp x-axis within the bounds of the box
            closestPoint.x = Mathf.Clamp(closestPoint.x, b.transform.position.x - b.transform.localScale.x * 0.5f, b.transform.position.x + b.transform.localScale.x * 0.5f);
            // clamp y-axis within the bounds of the box
            closestPoint.y = Mathf.Clamp(closestPoint.y, b.transform.position.y - b.transform.localScale.y * 0.5f, b.transform.position.y + b.transform.localScale.y * 0.5f);
            // clamp z-axis within the bounds of the box
            closestPoint.z = Mathf.Clamp(closestPoint.z, b.transform.position.z - b.transform.localScale.z * 0.5f, b.transform.position.z + b.transform.localScale.z * 0.5f);

            // check if there is a collision
            float distSquared = (closestPoint - a.transform.position).sqrMagnitude;
            float radius = a.transform.localScale.x * 0.5f;
            if (distSquared <= radius * radius)
            {
                Vector3 contactNormal = Vector3.zero;
                ResolveBoxSphereInterpenetration(b, a, closestPoint, out contactNormal);
                ResolveVelocity(b, a, contactNormal, restitutionConstant);
            }
        }
        // A is a box, B is a sphere
        else if (aBox != null && bBox == null)
        {
            // determine closest point from the sphere to the box
            Vector3 closestPoint = b.transform.position;
            // clamp x-axis within the bounds of the box
            closestPoint.x = Mathf.Clamp(closestPoint.x, a.transform.position.x - a.transform.localScale.x * 0.5f, a.transform.position.x + a.transform.localScale.x * 0.5f);
            // clamp y-axis within the bounds of the box
            closestPoint.y = Mathf.Clamp(closestPoint.y, a.transform.position.y - a.transform.localScale.y * 0.5f, a.transform.position.y + a.transform.localScale.y * 0.5f);
            // clamp z-axis within the bounds of the box
            closestPoint.z = Mathf.Clamp(closestPoint.z, a.transform.position.z - a.transform.localScale.z * 0.5f, a.transform.position.z + a.transform.localScale.z * 0.5f);

            // check if there is a collision
            float distSquared = (closestPoint - b.transform.position).sqrMagnitude;
            float radius = b.transform.localScale.x * 0.5f;
            if (distSquared <= radius * radius)
            {
                Vector3 contactNormal = Vector3.zero;
                ResolveBoxSphereInterpenetration(a, b, closestPoint, out contactNormal);
                ResolveVelocity(a, b, contactNormal, restitutionConstant);
            }
        }


    }

    // Resolves interpenetration between two colliding sphere particles.
    void ResolveSphereInterpenetration(Particle3D a, Particle3D b, float midline, Vector3 contactNormal)
    {
        // determine how much to move and in what direction
        float pen = a.transform.localScale.x * 0.5f + b.transform.localScale.x * 0.5f - midline;

        Vector3 movementPerImass = contactNormal * (pen / (a.getInverseMass() + b.getInverseMass()));
        movementPerImass *= 1.01f; // move the particle a little extra to make sure it's not still colliding

        // apply individual movements
        a.transform.position += movementPerImass * a.getInverseMass();
        b.transform.position -= movementPerImass * b.getInverseMass();
    }

    // Resolves interpenetration between two colliding box particles.
    // Got some help from here:
    // https://stackoverflow.com/questions/46172953/aabb-collision-resolution-slipping-sides
    void ResolveBoxInterpenetration(Particle3D a, Particle3D b, out Vector3 contactNormal)
    {
        // determine which axes are overlapping and by how much
        float distX = a.transform.position.x - b.transform.position.x;
        float scalesX = a.transform.localScale.x * 0.5f + b.transform.localScale.x * 0.5f;
        float distY = a.transform.position.y - b.transform.position.y;
        float scalesY = a.transform.localScale.y * 0.5f + b.transform.localScale.y * 0.5f;
        float distZ = a.transform.position.z - b.transform.position.z;
        float scalesZ = a.transform.localScale.z * 0.5f + b.transform.localScale.z * 0.5f;

        float penX, penY, penZ;
        if (distX > 0)
            penX = scalesX - distX;
        else
            penX = -scalesX - distX;
        if (distY > 0)
            penY = scalesY - distY;
        else
            penY = -scalesY - distY;
        if (distZ > 0)
            penZ = scalesZ - distZ;
        else
            penZ = -scalesZ - distZ;

        // set up contact normal
        // The axis with the smallest penetration is the one that collides.
        contactNormal = Vector3.zero;
        float min = Mathf.Min(Mathf.Abs(penX), Mathf.Min(Mathf.Abs(penY), Mathf.Abs(penZ)));
        float penToUse = 0f;
        if (min == Mathf.Abs(penX))
        {
            //print("X collision");
            contactNormal = Vector3.right;
            penToUse = penX;
        }
        else if (min == Mathf.Abs(penY))
        {
            //print("Y collision");
            contactNormal = Vector3.up;
            penToUse = penY;
        }
        else if (min == Mathf.Abs(penZ))
        {
            //print("Z collision");
            contactNormal = Vector3.forward;
            penToUse = penZ;
        }

        Vector3 movementPerImass = contactNormal * (penToUse / (a.getInverseMass() + b.getInverseMass()));
        movementPerImass *= 1.01f; // move the particle a little extra to make sure it's not still colliding

        // apply individual movements
        a.transform.position += movementPerImass * a.getInverseMass();
        b.transform.position -= movementPerImass * b.getInverseMass();
    }

    void ResolveBoxSphereInterpenetration(Particle3D box, Particle3D sphere, Vector3 closestPoint, out Vector3 contactNormal)
    {
        // calculate contact normal
        contactNormal = (sphere.transform.position - closestPoint).normalized;

        // determine how much to move
        float dist = (closestPoint - sphere.transform.position).magnitude;
        float radius = sphere.transform.localScale.x * 0.5f;
        float pen = radius - dist;

        Vector3 movementPerImass = contactNormal * (pen / (box.getInverseMass() + sphere.getInverseMass()));
        movementPerImass *= 1.01f; // move the particle a little extra to make sure it's not still colliding

        // apply individual movements
        box.transform.position -= movementPerImass * box.getInverseMass();
        sphere.transform.position += movementPerImass * sphere.getInverseMass();
    }

    // Finds the appropriate velocity for each particle in a collision.
    void ResolveVelocity(Particle3D a, Particle3D b, Vector3 contactNormal, float restitution)
    {
        // determine change in velocity
        float separationVelocity = Vector3.Dot(contactNormal, (a.getVelocity() - b.getVelocity()));
        float newSeparationVelocity = separationVelocity * -restitution;
        float dv = newSeparationVelocity - separationVelocity;
        float impulseVelocity = dv / (a.getInverseMass() + b.getInverseMass());
        Vector3 velocityPerInverseMass = contactNormal * impulseVelocity;

        // determine new velocities
        Vector3 aDelta = velocityPerInverseMass * a.getInverseMass();
        Vector3 bDelta = velocityPerInverseMass * -b.getInverseMass(); // b goes in the opposite direction from a

        // apply new velocities
        a.setVelocity(a.getVelocity() + aDelta);
        b.setVelocity(b.getVelocity() + bDelta);
    }

    // Determines if a particle is colliding with a plane.
    // Got some help from here:
    // https://gdbooks.gitbooks.io/3dcollisions/content/Chapter2/static_aabb_plane.html
    void CheckPlaneCollisions(Particle3D part, Plane[] planes)
    {
        // define data needed for calculations
        Vector3 originVector = part.transform.position;

        // determine if the particle is an AABB or a sphere
        AABB box = part.gameObject.GetComponent<AABB>();

        // check for a collision between the sphere and any of the planes
        if (box == null)
        {
            for (int i = 0; i < planes.Length; i++)
            {
                // determine if this plane intersects with the sphere
                float distToPlane = Vector3.Dot(originVector, planes[i].normal) - planes[i].originOffset;
                if (distToPlane * distToPlane < (part.transform.localScale.x * 0.5f) * (part.transform.localScale.x * 0.5f))
                {
                    ResolveSpherePlaneInterpenetration(part, planes[i], distToPlane);
                    ResolveVelocity(part, planes[i].particle, planes[i].normal, restitutionConstant);
                }
            }
        }
        // check for a collision between the AABB and any of the planes
        else
        {
            for (int i = 0; i < planes.Length; i++)
            {
                // determine if this plane intersects with the AABB

                // get half-widths of the AABB
                Vector3 halfWidths = box.getHalfSize();

                // project the farthest AABB vertex from the plane onto the plane's normal
                float maxLength = halfWidths.x * Mathf.Abs(planes[i].normal.x) +
                                  halfWidths.y * Mathf.Abs(planes[i].normal.y) +
                                  halfWidths.z * Mathf.Abs(planes[i].normal.z);

                // compute the distance from the plane to the particle
                float distToPlane = Vector3.Dot(originVector, planes[i].normal) - planes[i].originOffset;
                if (Mathf.Abs(distToPlane) <= maxLength)
                {
                    ResolveAABBPlaneInterpenetration(part, planes[i], maxLength, distToPlane);
                    ResolveVelocity(part, planes[i].particle, planes[i].normal, restitutionConstant);
                }
            }
        }
    }

    // Resolves interpenetration between a sphere and a plane.
    void ResolveSpherePlaneInterpenetration(Particle3D part, Plane plane, float distToPlane)
    {
        float penetration = distToPlane - part.transform.localScale.x * 0.5f;

        // move the particle
        part.transform.position -= penetration * plane.normal * 1.01f; // move the particle a little extra to make sure it's not still colliding
    }

    // Resolves interpenetration between an AABB and a plane.
    void ResolveAABBPlaneInterpenetration(Particle3D part, Plane plane, float AABBExtent, float distToPlane)
    {
        float penetration = distToPlane - AABBExtent;

        // move the particle
        part.transform.position -= penetration * plane.normal * 1.01f; // move the particle a little extra to make sure it's not still colliding
    }

    public void CollideSpheres(Particle3D a, Particle3D b, float midline, Vector3 contactNormal, float restitution)
    {
        ResolveSphereInterpenetration(a, b, midline, contactNormal);
        ResolveVelocity(a, b, contactNormal, restitution);
    }
}
