using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private EnemyHealth health;

    [Header("Movement Settings")]
    public float detectionRange = 20f;
    public float attackRange = 2f;
    public float repathMin = 0.4f;
    public float repathMax = 1.2f;
    public float chaseOffsetRadius = 2f;

    [Header("Attack Settings")]
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;

    private bool hasSeenPlayer = false;
    private float repathTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // Randomize avoidance priority so enemies don't stack
        agent.avoidancePriority = Random.Range(20, 80);
    }

    void Update()
    {
        if (health.currentHealth <= 0)
            return;

        attackTimer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        // Detect player
        if (!hasSeenPlayer && dist < detectionRange)
            hasSeenPlayer = true;

        if (!hasSeenPlayer)
        {
            Wander();
            return;
        }

        // Chase player with random offset
        ChasePlayer(dist);

        // Attack if close enough
        if (dist <= attackRange)
            TryAttack();
    }

    void ChasePlayer(float dist)
    {
        repathTimer -= Time.deltaTime;

        if (repathTimer <= 0f)
        {
            repathTimer = Random.Range(repathMin, repathMax);

            // Add random offset so enemies don't stack
            Vector3 offset = new Vector3(
                Random.Range(-chaseOffsetRadius, chaseOffsetRadius),
                0,
                Random.Range(-chaseOffsetRadius, chaseOffsetRadius)
            );

            agent.SetDestination(player.position + offset);
        }
    }

    void TryAttack()
    {
        if (attackTimer > 0f)
            return;

        attackTimer = attackCooldown;

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(attackDamage);
    }

    void Wander()
    {
        if (agent.remainingDistance < 0.5f)
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-5f, 5f),
                0,
                Random.Range(-5f, 5f)
            );

            agent.SetDestination(randomPos);
        }
    }
}
