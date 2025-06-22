using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Configuração de Ataque")]
    public float attackRange = 3.0f;   // Distância para parar
    public float hitRange = 1.5f;      // Alcance real de dano
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;

    [Header("Vida")]
    public int maxHealth = 50;
    private int currentHealth;

    private NavMeshAgent agent;
    private Animator anim;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // CORRETO: parar antes de encostar!
        agent.stoppingDistance = attackRange;
        agent.autoBraking = false;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Perseguir
            if (agent.isStopped)
                agent.isStopped = false;

            agent.SetDestination(player.position);
            anim.SetBool("isWalking", true);
        }
        else
        {
            // Parar de perseguir completamente
            if (!agent.isStopped)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }

            anim.SetBool("isWalking", false);

            // Girar para olhar para o player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Atacar se cooldown permitir
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                anim.SetTrigger("attack");
                lastAttackTime = Time.time;
            }
        }
    }

    // Evento na animação de ataque
    public void ApplyDamageToPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= hitRange)
        {
            PlayerDano health = player.GetComponent<PlayerDano>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
                Debug.Log("Player tomou dano do Skeleton!");
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Skeleton tomou dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Skeleton morreu!");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }
}
