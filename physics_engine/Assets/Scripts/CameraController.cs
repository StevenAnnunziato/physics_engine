using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float scrollSpeed = 10f;
    private GameObject cam;

    private void Start()
    {
        cam = transform.Find("Main Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            input *= sensitivity;
            Vector3 newRot = transform.rotation.eulerAngles + new Vector3(input.y, input.x, 0f);
            transform.rotation = Quaternion.Euler(newRot);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.transform.Translate(Vector3.forward * scroll * scrollSpeed * Time.deltaTime, Space.Self);
    }
}
