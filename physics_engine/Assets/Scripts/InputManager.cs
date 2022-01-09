using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    private Camera cam;

    [SerializeField] private float velocityMultiplier = 1f;

    private Vector3 startPos;
    private Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            startPos = ScreenToWorldPoint();
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = ScreenToWorldPoint();

            // create particle
            Particle2D part = Instantiate(particle, startPos, Quaternion.identity).GetComponent<Particle2D>();

            // give proper velocity to the particle
            part.setVelocity((endPos - startPos) * velocityMultiplier);
        }

    }

    private Vector3 ScreenToWorldPoint()
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f; // lock z-axis
        return worldPos;
    }
}
