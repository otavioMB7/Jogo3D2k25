using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Configuração de Ataque")]
    public float attackRange = 3.0f;  // distância para parar e atacar
    public float hitRange = 1.5f;     // alcance real para dar dano
    public float tempoDano = 1.5f;    // intervalo entre ataques

    private bool alreadyDealtDamage = false; // evitar dano duplo

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

        // MUITO IMPORTANTE: define a distância de parada do agente de navegação
        agent.stoppingDistance = attackRange * 0.9f; // um pouco menor para suavizar
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Perseguir player
            agent.SetDestination(player.position);

            anim.SetBool("isWalking", true);
            alreadyDealtDamage = false;
        }
        else
        {
            // Dentro da área de ataque: parar de andar
            anim.SetBool("isWalking", false);

            if (Time.time > lastAttackTime + tempoDano)
            {
                anim.SetTrigger("attack"); // Dispara animação de ataque
                lastAttackTime = Time.time;

                if (!alreadyDealtDamage)
                {
                    DealDamage();
                    alreadyDealtDamage = true;
                }
            }
        }
    }

    // Aplica dano apenas se dentro do alcance real de acerto
    void DealDamage()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= hitRange)
        {
            PlayerDano health = player.GetComponent<PlayerDano>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
        }
    }

    // Recebe dano de fora (ex: ataque do player)
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

    // (Opcional) Visualizar o alcance no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }
}


