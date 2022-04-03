using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
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

    [SerializeField]
    private float maxHP;
    private float currentHP;

    private float damage = 10;
    private float critMultiplier = 2f;
    private float critChance = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        // body = GetComponent<Rigidbody>();
        currentHP = maxHP;
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
        //float verticalSpeed = 0f;
        //if (transform.position.y > h)
        //{
        //    verticalSpeed = 3 * (h - transform.position.y);
        //}
        //else if (transform.position.y < h)
        //{
        //
        //    verticalSpeed = 3 * (h - transform.position.y);
        //}
        
        transform.localPosition = new Vector3(transform.position.x, h, transform.position.z);

        body.velocity = new Vector3(vel2.x, 0, vel2.y) * speed;
        // float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        // transform.position = new Vector3(transform.position.x, h, transform.position.z);
    }

    void OnMove(InputValue value)
    {
        moveAxis = value.Get<Vector2>();
        // Debug.Log(moveAxis);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EnemyBullet"))
        {
            Bullet bullet = collider.gameObject.GetComponent<Bullet>();
            currentHP -= bullet.GetDamage();
            Debug.Log($"Took damage {bullet.GetDamage()} now have {currentHP} hp.");
            Destroy(bullet.gameObject);
        }
        else if (collider.CompareTag("Loot"))
        {
            LootObject loot = collider.GetComponent<LootObject>();
            if (loot != null)
            {
                loot.GetPool().DeactivateObject(loot.gameObject);
                Upgrade(loot.GetConfig());
            }

        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"Took damage {amount} now have {currentHP} hp.");
    }

    private void Upgrade(LootConfig config)
    {
        speed += config.movementSpeed;
        damage += config.damage;
        critChance += config.crit;
        currentHP += config.healHP + config.maxHP;
        maxHP += config.maxHP;
    }

    public float CalcDamage()
    {
        float mult = Random.Range(0f, 1f) < critChance ? critMultiplier : 1;
        return ((float)damage) * mult;
    }
}
