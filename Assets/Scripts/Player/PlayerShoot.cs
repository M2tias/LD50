using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private PlayerMovement player;

    private bool shoot = false;
    
    void Start()
    {
    }

    void Update()
    {
        if (shoot)
        {
            GameObject instance = Instantiate(bulletPrefab);
            instance.transform.position = transform.position + transform.forward;
            Bullet bullet = instance.GetComponent<Bullet>();
            float damage = player.CalcDamage();
            Terrain terrain = GameManager.main.GetTerrain();
            bullet.Initialize(terrain, transform.localRotation * Vector3.forward, damage);
        }

        shoot = false;
    }

    void OnShoot(InputValue value)
    {
        shoot = true;
    }
}
