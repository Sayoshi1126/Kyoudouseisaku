using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMarker : MonoBehaviour
{
    GameObject camera;
    GameObject cameraMarker;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("CM vcam1");
        cameraMarker = transform.Find("CameraMarker").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = camera.transform.position;
        cameraMarker.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
    }
    private void LateUpdate()
    {

    }
}
