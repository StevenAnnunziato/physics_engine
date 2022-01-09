using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // clear all projectiles from the screen
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject[] particles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject part in particles)
            {
                if (part.GetComponent<Particle3D>().getInverseMass() != 0)
                    Destroy(part);
            }
        }

        // edit gravity with arrow keys
        CheckGravity();
    }

    private void CheckGravity()
    {
        const float GRAV = 10f;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject[] particles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject part in particles)
                part.GetComponent<Particle3D>().setGravity(Vector2.left * GRAV);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameObject[] particles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject part in particles)
                part.GetComponent<Particle3D>().setGravity(Vector2.right * GRAV);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameObject[] particles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject part in particles)
                part.GetComponent<Particle3D>().setGravity(Vector2.up * GRAV);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameObject[] particles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject part in particles)
                part.GetComponent<Particle3D>().setGravity(Vector2.down * GRAV);
        }
    }
}
