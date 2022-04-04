using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    [SerializeField]
    private PlayerMovement player;

    [SerializeField]
    private Terrain terrain;

    [SerializeField]
    private List<GameObject> firstSpawners;
    [SerializeField]
    private int lastFirstSpawnerTier;

    [SerializeField]
    private List<GameObject> secondSpawners;
    [SerializeField]
    private int lastSecondSpawnerTier;

    [SerializeField]
    private List<GameObject> thirdSpawners;
    [SerializeField]
    private int lastThirdSpawnerTier;

    // [SerializeField]
    // private GameObject probePrefab;
    // [SerializeField]
    // private GameObject fighterPrefab;

    [SerializeField]
    private List<int> probesPerTier;

    [SerializeField]
    private List<int> fightersPerTier;

    [SerializeField]
    private List<int> bigFightersPerTier;
    [SerializeField]
    private ObjectPool probePool;
    [SerializeField]
    private ObjectPool fighterPool;
    [SerializeField]
    private ObjectPool bigFighterPool;
    [SerializeField]
    private List<ObjectPool> objectPools;
    [SerializeField]
    private float chanceToDropLoot;
    [SerializeField]
    private ScoreScriptableObject scoreObject;


    private int currentPhase = 0;
    // Tier advancement
    private int currentTier = 0;
    private float timeToAdvanceNextTierBase = 60f;
    private float timeToAdvanceNextTierMultiplierTime = 5f;
    private float lastTierAdvanceTime = 0f;

    // Spawning
    private float timeBetweenSpawnsBase = 13f; // TODO: increase/decrease?
    private float timeBetweenSpawnsMultiplier = 0.975f;
    private float lastSpawnTime = 0f;
    private float probesLeftToSpawn = 0f;
    private float fightersLeftToSpawn = 0f;
    private float bigFightersLeftToSpawn = 0f;
    private List<GameObject> currentSpawners;

    private int killCount = 0;

    void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastTierAdvanceTime = Time.time;
        lastSpawnTime = Time.time - 5f;
        scoreObject.killCount = 0;
        scoreObject.lastPhase = 0;
        scoreObject.lastTier = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float timeToSpawn = timeBetweenSpawnsBase * Mathf.Pow(timeBetweenSpawnsMultiplier, currentTier);
        if (Time.time - lastSpawnTime > timeBetweenSpawnsBase)
        {
            Spawn();
            lastSpawnTime = Time.time;
            currentPhase++;
        }

        float timeToAdvance = timeToAdvanceNextTierBase + timeToAdvanceNextTierMultiplierTime * currentTier;

        if (Time.time - lastTierAdvanceTime > timeToAdvance)
        {
            currentTier++;
            lastTierAdvanceTime = Time.time;
        }

        scoreObject.killCount = killCount;
        scoreObject.lastPhase = currentPhase;
        scoreObject.lastTier = currentTier;
    }

    public Terrain GetTerrain()
    {
        return terrain;
    }

    private void Spawn()
    {
        probesLeftToSpawn = probesPerTier[currentPhase];
        fightersLeftToSpawn = fightersPerTier[currentPhase];
        bigFightersLeftToSpawn = bigFightersPerTier[currentPhase];

        if (currentTier <= lastFirstSpawnerTier)
        {
            currentSpawners = firstSpawners;
        }
        else if (currentTier <= lastSecondSpawnerTier)
        {
            currentSpawners = secondSpawners;
        }
        else
        {
            currentSpawners = thirdSpawners;
        }
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int i = 0;
        while (probesLeftToSpawn > 0 || fightersLeftToSpawn > 0 || bigFightersLeftToSpawn > 0)
        {
            GameObject spawner = currentSpawners[Random.Range(0, currentSpawners.Count)];
            if (i % 3 == 0 && probesLeftToSpawn > 0)
            {
                // Spawn probe
                GameObject obj = probePool.ActivateObject();
                float h = GameManager.main.GetTerrain().SampleHeight(spawner.transform.position) + 0.25f;
                obj.transform.position = new Vector3(spawner.transform.position.x, h, spawner.transform.position.z);
                obj.GetComponent<NavMeshAgent>().enabled = true;

                EnemyMovement probe = obj.GetComponent<EnemyMovement>();
                probe.enabled = true;
                probe.Initialize(player, probePool);
                probesLeftToSpawn--;
            }
            else if (i % 3 == 1 && fightersLeftToSpawn > 0)
            {
                // Spawn fighter
                GameObject obj = fighterPool.ActivateObject();
                float h = GameManager.main.GetTerrain().SampleHeight(spawner.transform.position) + 0.25f;
                obj.transform.position = new Vector3(spawner.transform.position.x, h, spawner.transform.position.z);
                obj.GetComponent<NavMeshAgent>().enabled = true;

                FighterMovement fighter = obj.GetComponent<FighterMovement>();
                fighter.enabled = true;
                fighter.Initialize(player, fighterPool);
                fightersLeftToSpawn--;
            }
            else if (bigFightersLeftToSpawn > 0)
            {
                // Spawn big fighter
                GameObject obj = bigFighterPool.ActivateObject();
                float h = GameManager.main.GetTerrain().SampleHeight(spawner.transform.position) + 0.25f;
                obj.transform.position = new Vector3(spawner.transform.position.x, h, spawner.transform.position.z);
                obj.GetComponent<NavMeshAgent>().enabled = true;

                BigFighterMovement bigFighter = obj.GetComponent<BigFighterMovement>();
                bigFighter.enabled = true;
                bigFighter.Initialize(player, bigFighterPool);
                bigFightersLeftToSpawn--;

            }
            i++;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void DropLoot(Vector3 pos)
    {
        if (Random.Range(0f, 1f) < chanceToDropLoot)
        {
            ObjectPool pool = objectPools[Random.Range(0, objectPools.Count)];
            GameObject loot = pool.ActivateObject();
            loot.GetComponent<LootObject>().Initialize(pool);
            loot.transform.position = pos;
        }
    }

    public void EnemyKilled()
    {
        killCount++;
    }

    public float GetCurrentHP()
    {
        return player.GetCurrentHP();
    }

    public float GetMaxHP()
    {
        return player.GetMaxHP();
    }

    public int GetCurrentTier()
    {
        return currentTier;
    }
    public int GetCurrentPhase()
    {
        return currentPhase;
    }

    public float GetTierDamageMultiplier()
    {
        return (0.30f * currentTier) + 1f;
    }
}
