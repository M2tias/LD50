using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody body;
    [SerializeField]
    private float damage;

    Terrain terrain;

    Vector3 dir;
    private float timeStarted;
    private float lifeTime = 5f;
    private bool initialized = false;

    void Start()
    {

    }

    public void Initialize(Terrain terrain, Vector3 dir, float? damage = null)
    {
        if (damage.HasValue)
        {
            this.damage = damage.Value;
        }
        this.dir = dir;
        this.terrain = terrain;
        timeStarted = Time.time;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;
        float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        float verticalSpeed = 0f;
        Vector3 tp = transform.localPosition;
        transform.localPosition = new Vector3(tp.x, h, tp.z);

        body.velocity = new Vector3(dir.x, 0, dir.z) * speed + new Vector3(0, verticalSpeed, 0);

    }

    public float GetDamage()
    {
        return damage;
    }
}
