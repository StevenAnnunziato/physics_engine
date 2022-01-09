using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField] private bool shouldSway;
    [SerializeField] private float swayFrequency = 2f;
    [SerializeField] private float swayAmplitude = 0.5f;
    [SerializeField] private int scoreReward = 1;
    private float swayOffset;
    private ScoreManager sm;

    private Particle2D part;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<Particle2D>();
        sm = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();

        swayOffset = Random.Range(0f, 6.28f);
    }

    // Update is called once per frame
    void Update()
    {

        // animate sway
        if (shouldSway)
        {
            float swayFactor = Mathf.Sin(Time.time * swayFrequency + swayOffset) * swayAmplitude;
            part.setVelocity(new Vector2(swayFactor, part.getVelocity().y));
        }

        // get all projectiles
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        // loop through all of them
        for (int i = 0; i < projectiles.Length; i++)
        {
            // check for a collision
            float dist = Vector3.Distance(transform.position, projectiles[i].transform.position);
            float radius = transform.localScale.x + projectiles[i].transform.localScale.x; // assumed to be spherical
            if (dist < radius) // found collision
            {
                print("pop"); // ideally this would be a sound effect

                // update score
                sm.AddScore(scoreReward);

                // cleanup
                Destroy(projectiles[i]);
                Destroy(gameObject);
            }
        }
        
    }
}
