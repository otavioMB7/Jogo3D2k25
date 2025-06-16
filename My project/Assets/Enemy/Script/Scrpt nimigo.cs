using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Configurações de Ataque")]
    public float attackDistance = 2.5f;
    public float tempoDano = 1.5f;

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
        agent.stoppingDistance = attackDistance;

        currentHealth = maxHealth;
    }

    void Update()
    {
        anim.SetBool("isWalking", true);
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        agent.SetDestination(player.position);

        if (distance > agent.stoppingDistance)
        {
            // Está longe: anda até o player
            anim.SetBool("isWalking", true);
        }
        else
        {
            // Está perto: para e ataca
            anim.SetBool("isWalking", false);

            // Olha para o player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Ataca se passou o tempo do cooldown
            if (Time.time > lastAttackTime + tempoDano)
            {
                anim.SetTrigger("attack");   // Dispara a animação de ataque
                lastAttackTime = Time.time;

                DealDamage();
            }
        }
    }

    void DealDamage()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackDistance)
        {
            PlayerDano health = player.GetComponent<PlayerDano>();
            if (health != null)
            {
                health.TakeDamage(10);
                Debug.Log("Skeleton causou dano ao Player!");
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
}
