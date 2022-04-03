using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSound : MonoBehaviour
{
    private float started;
    // Start is called before the first frame update
    void Start()
    {
        started = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - started >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
