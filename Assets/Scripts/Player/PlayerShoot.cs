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
    [SerializeField]
    private AudioSource audio;

    private bool shoot = false;
    private float lastAttackTime = 0f;

    void Start()
    {
    }

    void Update()
    {
        if (shoot && Time.time - lastAttackTime > player.GetTimeBetweenAttacks())
        {
            GameObject instance = Instantiate(bulletPrefab);
            instance.transform.position = transform.position + transform.forward;
            Bullet bullet = instance.GetComponent<Bullet>();
            float damage = player.CalcDamage();
            Terrain terrain = GameManager.main.GetTerrain();
            bullet.Initialize(terrain, transform.localRotation * Vector3.forward, damage);
            lastAttackTime = Time.time;
            audio.Play();
        }
    }

    void OnShoot(InputValue value)
    {
        shoot = true;
    }

    void OnStopshooting(InputValue value)
    {
        shoot = false;
    }
}
