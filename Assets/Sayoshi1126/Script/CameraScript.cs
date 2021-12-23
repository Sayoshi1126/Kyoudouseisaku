using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rigidbody2D;
    Vector2 lastPosition;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Settings.Instance.cameraX = transform.position.x;
        Settings.Instance.cameraY = transform.position.y;

        Settings.Instance.cameraVX = rigidbody2D.velocity.x - lastPosition.x; ;
        Settings.Instance.cameraVY = rigidbody2D.velocity.y - lastPosition.y;

        lastPosition = transform.position;
    }
}
