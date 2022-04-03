using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private ObjectPool pool;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(ObjectPool pool)
    {
        currentHP = maxHP;
        this.pool = pool;
    }

    // Update is called once per frame
    void Update()
    {
        if (pool == null) return;

        if (currentHP <= 0)
        {
            GameManager.main.DropLoot(transform.position);
            pool.DeactivateObject(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            Bullet bullet = collider.gameObject.GetComponent<Bullet>();
            currentHP -= bullet.GetDamage();
            Debug.Log($"Took damage {bullet.GetDamage()} now have {currentHP} hp.");
            Destroy(bullet.gameObject);
        }
    }
}
