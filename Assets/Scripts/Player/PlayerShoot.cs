using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;

    [SerializeField]
    private GameObject bulletPrefab;

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
            bullet.Initialize(terrain, transform.localRotation * Vector3.forward);
        }

        shoot = false;
    }

    void OnShoot(InputValue value)
    {
        shoot = true;
    }
}
