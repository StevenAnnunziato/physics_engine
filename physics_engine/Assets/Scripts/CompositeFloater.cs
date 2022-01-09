using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeFloater : MonoBehaviour
{
    // NOTE: There need to be at least 3 floaties
    public GameObject[] floaties;
    public GameObject center;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // set center to be at the middle of all floaties
        Vector3 avg = Vector3.zero;
        foreach (GameObject go in floaties)
        {
            avg += go.transform.position;
        }
        avg /= floaties.Length;
        center.transform.position = avg;

        // set center rotation
        Vector3 vec1 = floaties[1].transform.position - floaties[0].transform.position;
        Vector3 vec2 = floaties[2].transform.position - floaties[0].transform.position;
        //center.transform.up = Vector3.Cross(vec1, vec2);
        //center.transform.forward = vec1;
        center.transform.rotation = Quaternion.LookRotation(vec1, Vector3.Cross(vec1, vec2));
    }
}
