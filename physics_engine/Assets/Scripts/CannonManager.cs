using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField] private float cooldown = 0.3f;
    private float currentCooldown;
    [SerializeField] private float scrollSpeed = 10f;
    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject reticle;
    private int numProjectiles = 0;
    private int currentProjectile = 0;
    private float planeHeight = 5f;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        numProjectiles = projectiles.Length;
    }

    // Update is called once per frame
    void Update()
    {

        // allow the user to scroll the mouse wheel to raise/lower reticle
        planeHeight += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        planeHeight = Mathf.Clamp(planeHeight, -5f, 5f); // bound to the box

        // find where the user is aiming
        Vector3 hitPosition = Vector3.up * 200f; // get it away from view
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, planeHeight);
        if (plane.Raycast(ray, out float distance))
        {
            hitPosition = ray.GetPoint(distance);
            // bound to the box
            hitPosition.x = Mathf.Clamp(hitPosition.x, -5f, 5f);
            hitPosition.z = Mathf.Clamp(hitPosition.z, -5f, 5f);
        }
        // set a visual to appear at this location and aim the gun
        reticle.transform.position = hitPosition;
        objectToRotate.transform.LookAt(hitPosition);

        // swap projectiles
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentProjectile = ++currentProjectile % numProjectiles;
        }

        // fire
        if (currentCooldown <= 0f && Input.GetKeyDown(KeyCode.Return))
        {
            FireProjectile();
        }

        // tick cooldown
        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;
    }

    void FireProjectile()
    {
        // reset cooldown
        currentCooldown = cooldown;

        Particle3D part = null;

        // give proper attributes to the particle
        switch (currentProjectile)
        {
            case 0: // bowling ball
                part = Instantiate(projectiles[0], spawnPos.position, Quaternion.identity).GetComponent<Particle3D>();
                part.setVelocity(spawnPos.transform.forward * 5f);
                part.setDampingConstant(0.5f);
                break;
            case 1: // baseball?
                part = Instantiate(projectiles[1], spawnPos.position, Quaternion.identity).GetComponent<Particle3D>();
                part.setVelocity(spawnPos.transform.forward * 8f);
                part.setDampingConstant(0.7f);
                break;
            case 2: // marble
                part = Instantiate(projectiles[2], spawnPos.position, Quaternion.identity).GetComponent<Particle3D>();
                part.setVelocity(spawnPos.transform.forward * 10f);
                part.setDampingConstant(0.9f);
                break;
            case 3: // AABB
                part = Instantiate(projectiles[3], spawnPos.position, Quaternion.identity).GetComponent<Particle3D>();
                part.setVelocity(spawnPos.transform.forward * 8f);
                part.setDampingConstant(0.8f);
                break;
            default:
                Debug.LogError("Error: projectile type not found.");
                break;
        }

    }
}
