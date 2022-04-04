using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigFighterMovement : MonoBehaviour, IDeinitialize
{
    private PlayerMovement player;
    private NavMeshAgent agent;
    private EnemyHP enemyHP;
    private float lastDestinationTime = 0f;
    private float betweenDestinationChecks = 3f;

    [SerializeField]
    private GameObject model;

    [SerializeField]
    private bool startParticles = true;

    [SerializeField]
    private ParticleSystem leftMuzzleFlashParticles;

    [SerializeField]
    private ParticleSystem rightMuzzleFlashParticles;

    [SerializeField]
    private Light particleLight;

    [SerializeField]
    private GameObject bullerPrefab;

    private FighterState state = FighterState.Patrolling;

    // patrolling
    private float randomPointRange = 20f;
    private bool patrolling = false;
    private Vector3 patrolTarget;
    private float movementStalledStartTime = 0f;
    private float timeToStayStationary = 2f;
    private bool startedMoving = false;
    private bool stalled = false;

    // waiting
    private float waitStarted = 0f;
    private float waitDuration = 2f;

    // attacking
    private float rangeToStartAttack = 25f;
    private float updateDestinationWait = 1f;
    private float lastUpdatedDestinationTime = 0f;
    private float attackTime = 1f;
    private float lastAttackedTime = 0f;
    private float attackRange = 15f;
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource spawnSound;

    void Start()
    {
        // player = FindObjectOfType<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        enemyHP = GetComponent<EnemyHP>();
    }

    public void Initialize(PlayerMovement player, ObjectPool pool)
    {
        this.player = player;
        enemyHP ??= GetComponent<EnemyHP>();
        enemyHP.Initialize(pool);
        state = FighterState.Attacking;
        audioSource = GetComponent<AudioSource>();
        spawnSound.Play();
    }

    public void Deinitialize()
    {
        agent.enabled = false;
        state = FighterState.Patrolling;
    }

    void Update()
    {
        if (player == null)
        {
            // wait for player ref
            return;
        }

        if (state == FighterState.Attacking)
        {
            if (Vector2.Distance(Vec2(transform.position), Vec2(player.transform.position)) < attackRange)
            {
                Quaternion x = Quaternion.Euler(0, Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up).eulerAngles.y, 0);
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, x, 5f);

                if (Time.time - lastAttackedTime > attackTime)
                {
                    GameObject bullet = Instantiate(bullerPrefab);
                    bullet.transform.position = leftMuzzleFlashParticles.transform.position + transform.forward * 1.5f;
                    leftMuzzleFlashParticles.Play();
                    GameObject bullet2 = Instantiate(bullerPrefab);
                    bullet2.transform.position = rightMuzzleFlashParticles.transform.position + transform.forward * 1.5f;
                    rightMuzzleFlashParticles.Play();

                    bullet.GetComponent<Bullet>().Initialize(GameManager.main.GetTerrain(), transform.forward, 20 * GameManager.main.GetTierDamageMultiplier());
                    bullet2.GetComponent<Bullet>().Initialize(GameManager.main.GetTerrain(), transform.forward, 20 * GameManager.main.GetTierDamageMultiplier());
                    lastAttackedTime = Time.time;
                    audioSource.Play();
                }
            }
            else
            {
                if (Time.time - lastUpdatedDestinationTime > updateDestinationWait)
                {
                    agent.SetDestination(player.transform.position);
                    lastUpdatedDestinationTime = Time.time;
                }
            }
        }
        else if (state == FighterState.Patrolling)
        {
            if (patrolling && agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                patrolling = false;
            }

            if (!patrolling)
            {
                Vector2 unitCirclePoint = Random.insideUnitCircle.normalized * randomPointRange;
                float h = GameManager.main.GetTerrain().SampleHeight(unitCirclePoint);
                Vector2 dirTowardsPlayer = Vec2(player.transform.position - transform.position).normalized;
                patrolTarget = new Vector3(unitCirclePoint.x + transform.position.x + dirTowardsPlayer.x, h, unitCirclePoint.y + transform.position.z + dirTowardsPlayer.y);
                agent.SetDestination(patrolTarget);
                patrolling = true;
                startedMoving = false;
                movementStalledStartTime = Time.time;
                stalled = false;
            }

            if (Vector2.Distance(Vec2(patrolTarget), Vec2(transform.position)) < 1f || (!agent.pathPending && Vector2.Distance(Vec2(agent.pathEndPosition), Vec2(transform.position)) < 1f))
            {
                state = FighterState.Waiting;
                patrolling = false;
                waitStarted = Time.time;
                startedMoving = false;
                stalled = false;
                agent.SetDestination(transform.position);
            }

            // If the agent can't reach its target, wait and start again
            if (!startedMoving && agent.velocity.magnitude > 0.05f)
            {
                startedMoving = true;
            }

            if (!stalled && startedMoving && agent.velocity.magnitude < 0.05f)
            {
                movementStalledStartTime = Time.time;
                stalled = true;
            }

            if (stalled && (Time.time - movementStalledStartTime > timeToStayStationary))
            {
                state = FighterState.Waiting;
                patrolling = false;
                waitStarted = Time.time;
                startedMoving = false;
                stalled = false;
                agent.SetDestination(transform.position);
            }
        }
        else
        {
            if (Time.time - waitStarted > waitDuration)
            {
                state = FighterState.Patrolling;
            }
        }

        if (Vector2.Angle(Vec2(transform.forward), Vec2(player.transform.position - transform.position)) > 15)
        {
            if (Vector2.Distance(Vec2(transform.position), Vec2(player.transform.position)) < rangeToStartAttack)
            {
                state = FighterState.Attacking;
            }
        }
        else
        {
        }
    }

    private Vector2 Vec2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

}
