using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody body;

    [SerializeField]
    GameObject aimIndicator;

    [SerializeField]
    GameObject moveDirObject;

    [SerializeField]
    Terrain terrain;

    private Vector2 moveAxis;

    // Start is called before the first frame update
    void Start()
    {
        // body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        // Debug.Log(h);
        Debug.DrawLine(new Vector3(transform.position.x, 100, transform.position.z), new Vector3(transform.position.x, h, transform.position.z), Color.red);
        Debug.DrawLine(new Vector3(transform.position.x, 100, transform.position.z), new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z + 0.1f), Color.blue);
    }

    void FixedUpdate()
    {
        Vector3 dir = moveDirObject.transform.forward;
        Vector2 dir2 = new Vector2(dir.x, dir.z);
        Vector2 dir2p = Vector2.Perpendicular(dir2);
        Vector2 vel2 = (dir2 * moveAxis.y + dir2p * -1f * moveAxis.x).normalized;

        float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        float verticalSpeed = 0f;
        if (transform.position.y > h)
        {
            verticalSpeed = 3 * (h - transform.position.y);
        }
        else if (transform.position.y < h)
        {

            verticalSpeed = 3 * (h - transform.position.y);
        }

        body.velocity = new Vector3(vel2.x, verticalSpeed, vel2.y) * speed;
        // float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        // transform.position = new Vector3(transform.position.x, h, transform.position.z);
    }

    void OnMove(InputValue value)
    {
        moveAxis = value.Get<Vector2>();
        // Debug.Log(moveAxis);
    }
}
