using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOutput : MonoBehaviour
{

    public Particle3D a;
    public Particle3D b;

    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Particle3D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*float penX = Mathf.Abs((a.transform.position.x - a.transform.localScale.x * 0.5f) - (b.transform.position.x + b.transform.localScale.x * 0.5f));
        float penY = Mathf.Abs((a.transform.position.y - a.transform.localScale.y * 0.5f) - (b.transform.position.y + b.transform.localScale.y * 0.5f));
        float penZ = Mathf.Abs((a.transform.position.z - a.transform.localScale.z * 0.5f) - (b.transform.position.z + b.transform.localScale.z * 0.5f));

        // set up contact normal
        Vector3 contactNormal = new Vector3(penX, penY, penZ);
        print(contactNormal);*/

        //print((a.transform.position - a.transform.localScale * 0.5f) - (b.transform.position + b.transform.localScale * 0.5f));

        if (Mathf.Abs(a.transform.position.x - b.transform.position.x) < a.transform.localScale.x * 0.5f + b.transform.localScale.x * 0.5 &&
            Mathf.Abs(a.transform.position.y - b.transform.position.y) < a.transform.localScale.y * 0.5f + b.transform.localScale.y * 0.5 &&
            Mathf.Abs(a.transform.position.z - b.transform.position.z) < a.transform.localScale.z * 0.5f + b.transform.localScale.z * 0.5)
        {
            float distX = a.transform.position.x - b.transform.position.x;
            float scalesX = a.transform.localScale.x * 0.5f + b.transform.localScale.x * 0.5f;

            float x;
            if (distX > 0)
                x = scalesX - distX;
            else
                x = -scalesX - distX;

            //float x = (a.transform.localScale.x * 0.5f + b.transform.localScale.x * 0.5f) - Mathf.Abs(b.transform.position.x - a.transform.position.x);
            print(x);
        }
    }
}
