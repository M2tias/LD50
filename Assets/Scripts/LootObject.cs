using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootObject : MonoBehaviour, IDeinitialize
{
    // Loot types
    //   Rocket - higher damage
    //   Bottle - move faster
    //   Wrench - get hp
    //   Cog - get max hp (add currentHP same amount)
    //   Telescope - crit?

    [SerializeField]
    private LootConfig config;
    [SerializeField]
    private GameObject lootSoundPrefab;

    private GameObject model;
    private ObjectPool pool;

    // Start is called before the first frame update
    void Start()
    {
        // This needs to be done once and never again
        model = Instantiate(config.model);
        model.transform.parent = transform;
        model.transform.position = transform.position;
    }

    public void Deinitialize()
    {
    }

    public void Initialize(ObjectPool pool)
    {
        this.pool = pool;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public LootConfig GetConfig()
    {
        return config;
    }

    public ObjectPool GetPool()
    {
        return pool;
    }

    public void Pickup()
    {
        Instantiate(lootSoundPrefab).transform.parent = null;
    }
}
