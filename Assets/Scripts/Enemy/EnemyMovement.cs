using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IDeinitialize
{
    private PlayerMovement player;
    private NavMeshAgent agent;
    private EnemyHP enemyHP;
    private float lastDestinationTime = 0f;
    private float betweenDestinationChecks = 3f;

    [SerializeField]
    private int damage;

    [SerializeField]
    private GameObject model;
    [SerializeField]
    private bool startParticles = true;
    [SerializeField]
    private ParticleSystem electricityParticles;
    [SerializeField]
    private Light particleLight;
    private float particlesOnTime = 0.35f;
    private float particlesStartedTime = 0f;


    private ProbeState state = ProbeState.Detecting;
    private float probeStartedTime = 0f;
    private float probingTime = 1f; // check the player out for 2s
    private float detectStartedTime = 0f;
    private float detectingTime = 4; // look for player for 10s
    private float detectNewRotationStartedTime = 0f;
    private float detectRotationTime = 1f;
    private Quaternion targetRotation;

    private float timeBetweenAttacks = 2f;
    private float lastAttackTime = 0f;

    private float timeAfterPlayerWentOutOfReach = 0f;
    private float chaseTimeIfPlayerIsOutOfReach = 10f;
    private float infiniteChaseRange = 300f; // out of reach range, squared
    private bool playerOutOfReach = true;
    private AudioSource audio;
    [SerializeField]
    private AudioSource spawnSound;

    void Start()
    {
        // player = FindObjectOfType<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        detectStartedTime = Time.time;
        enemyHP = GetComponent<EnemyHP>();
    }

    public void Initialize(PlayerMovement player, ObjectPool pool)
    {
        this.player = player;
        enemyHP ??= GetComponent<EnemyHP>();
        enemyHP.Initialize(pool);
        audio = GetComponent<AudioSource>();
        spawnSound.Play();
    }

    public void Deinitialize()
    {
        agent.enabled = false;
        state = ProbeState.Detecting;
    }

    void Update()
    {
        if (player == null)
        {
            // wait for player ref
            return;
        }

        // Debug.Log(state);
        if (state == ProbeState.Attacking)
        {
            if (Vector3.SqrMagnitude(player.transform.position - transform.position) > infiniteChaseRange)
            {
                if (!playerOutOfReach)
                {
                    playerOutOfReach = true;
                    timeAfterPlayerWentOutOfReach = Time.time;
                }

                if (Time.time - timeAfterPlayerWentOutOfReach > chaseTimeIfPlayerIsOutOfReach)
                {
                    state = ProbeState.Detecting;
                    detectStartedTime = Time.time;
                    agent.SetDestination(transform.position); // no moving during detecting
                }
            }

            if (Time.time - lastDestinationTime > betweenDestinationChecks)
            {
                agent.SetDestination(player.transform.position);
            }

            if (Vector3.SqrMagnitude(player.transform.position - transform.position) < 16f)
            {
                if (Time.time - lastAttackTime > timeBetweenAttacks)
                {
                    startParticles = true;
                    lastAttackTime = Time.time;
                }

                if (startParticles)
                {
                    electricityParticles.Play();
                    particleLight.enabled = true;
                    particlesStartedTime = Time.time;
                    startParticles = false;
                    player.TakeDamage(damage * GameManager.main.GetTierDamageMultiplier());
                    audio.Play();
                }
            }
        }
        else if (state == ProbeState.Detecting)
        {
            if (Time.time - detectNewRotationStartedTime > detectRotationTime)
            {
                targetRotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
                detectNewRotationStartedTime = Time.time;
            }

            model.transform.localRotation = Quaternion.RotateTowards(model.transform.localRotation, targetRotation, 3f);

            if (Time.time - detectStartedTime > detectingTime)
            {
                state = ProbeState.Probing;
                probeStartedTime = Time.time;
                agent.SetDestination(transform.position); // no moving during probing
            }
        }
        else // looking towards player
        {
            Quaternion x = Quaternion.Euler(0, Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up).eulerAngles.y, 0);
            model.transform.localRotation = Quaternion.RotateTowards(model.transform.localRotation, x, 3f);
            if (Time.time - probeStartedTime > probingTime)
            {
                state = ProbeState.Attacking;
            }

            if (Vector3.SqrMagnitude(player.transform.position - transform.position) < infiniteChaseRange)
            {
                playerOutOfReach = false;
            }
        }

        if (Time.time - particlesStartedTime > particlesOnTime)
        {
            electricityParticles.Stop();
            particleLight.enabled = false;
        }
    }


}

enum ProbeState
{
    Detecting,
    Probing,
    Attacking
}
