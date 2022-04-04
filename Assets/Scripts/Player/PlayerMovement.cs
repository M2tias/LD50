using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private float damage = 8;
    private float critMultiplier = 2f;
    private float critChance = 0.05f;

    private float timeBetweenAttacks = 1f;
    private float timeBeforeEnd = 1.5f;
    private float endStartedTime;
    private bool dead = false;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        // body = GetComponent<Rigidbody>();
        currentHP = maxHP;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
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
            verticalSpeed = 0.2f * (h - transform.position.y);
        }
        else if (transform.position.y < h)
        {

            verticalSpeed = 0.2f * (h - transform.position.y);
        }

        transform.localPosition = new Vector3(transform.position.x, h, transform.position.z);

        body.velocity = new Vector3(vel2.x, 0, vel2.y) * speed;
        // float h = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        // transform.position = new Vector3(transform.position.x, h, transform.position.z);

        if (dead && Time.time - endStartedTime > timeBeforeEnd)
        {
            Debug.Log("DEAD");
            SceneManager.LoadScene(1);
        }
    }

    void OnMove(InputValue value)
    {
        moveAxis = value.Get<Vector2>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EnemyBullet"))
        {
            Bullet bullet = collider.gameObject.GetComponent<Bullet>();
            TakeDamage(bullet.GetDamage());
            Destroy(bullet.gameObject);
        }
        else if (collider.CompareTag("Loot"))
        {
            LootObject loot = collider.GetComponent<LootObject>();
            if (loot != null)
            {
                loot.Pickup();
                loot.GetPool().DeactivateObject(loot.gameObject);
                Upgrade(loot.GetConfig());
            }

        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        UIManager.main.UpdateHP();
        audio.Play();
        if (currentHP <= 0)
        {
            Debug.Log("SOON DEAD");
            endStartedTime = Time.time;
            dead = true;
        }
    }

    private void Upgrade(LootConfig config)
    {
        speed = Mathf.Min(speed + config.movementSpeed, 23);
        damage += config.damage;
        timeBetweenAttacks = timeBetweenAttacks * (1 - config.attackSpeed);
        critChance += config.crit;
        maxHP += config.maxHP;
        currentHP = Mathf.Min(currentHP + config.healHP + config.maxHP, maxHP);
        UIManager.main.ShowLootText(config.lootText);
        UIManager.main.UpdatePickedUpLootCounter(config.type);
        UIManager.main.UpdateHP();
    }

    public float CalcDamage()
    {
        float mult = Random.Range(0f, 1f) < critChance ? critMultiplier : 1;
        return ((float)damage) * mult;
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }

    public float GetTimeBetweenAttacks()
    {
        return timeBetweenAttacks;
    }
}
