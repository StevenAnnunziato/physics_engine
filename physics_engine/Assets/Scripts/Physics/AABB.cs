using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour
{

    // NOTE:
    // Center point is stored as transform.position, and
    // half widths are stored in transform.scale.

    // getters
    public Vector3 getCenterPoint() { return transform.position; }
    public Vector3 getHalfSize() { return transform.localScale * 0.5f; }
    public Vector3 getFullSize() { return transform.localScale; }

    // setters
    public void setCenterPoint(Vector3 val) { transform.position = val; }
    public void setHalfSize(Vector3 val) { transform.localScale = val * 2f; }
    public void setFullSize(Vector3 val) { transform.localScale = val; }

}
