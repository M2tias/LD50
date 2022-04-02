using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody body;

    Terrain terrain;

    Vector3 dir;

    void Start()
    {

    }

    public void Initialize(Terrain terrain, Vector3 dir)
    {
        this.dir = dir;
        this.terrain = terrain;
    }

    void Update()
    {
        float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        float verticalSpeed = 0f;
        // if (transform.position.y > h)
        // {
        //     verticalSpeed = 60 * (h - transform.position.y);
        // }
        // else if (transform.position.y < h)
        // {

        //     verticalSpeed = 60 * (h - transform.position.y);
        // }
        Vector3 tp = transform.localPosition;
        transform.localPosition = new Vector3(tp.x, h, tp.z);

        body.velocity = new Vector3(dir.x, 0, dir.z) * speed + new Vector3(0, verticalSpeed, 0);
        
    }
}
