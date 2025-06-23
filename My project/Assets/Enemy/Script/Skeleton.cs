using UnityEngine;
using UnityEngine.AI;

public class Skeleton: MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Collider attackHitbox; // Arraste o Collider aqui no Inspector

    [Header("Configuração de Ataque")]
    public float attackRange = 3.0f;
    public float attackCooldown = 1.5f;

    [Header("Vida")]
    public int maxHealth = 50;
    private int currentHealth;

    private NavMeshAgent agent;
    private Animator anim;

    private bool isAttacking;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        agent.stoppingDistance = attackRange;
        attackHitbox.enabled = false; // garante que comece desativada
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distance > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool("isWalking", true);
            }
            else
            {
                agent.isStopped = true;
                anim.SetBool("isWalking", false);

                // Olhar para o player
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }

                // Começar ataque
                StartCoroutine(Attack());
            }
        }
    }

    private System.Collections.IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // Animation Event
    public void EnableHitbox() => attackHitbox.enabled = true;
    public void DisableHitbox() => attackHitbox.enabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqui você pode acessar o script do player e aplicar dano
            PlayerDano health = other.GetComponent<PlayerDano>();
            if (health != null)
            {
                health.TakeDamage(10);
                Debug.Log("Player tomou dano do Skeleton via hitbox!");
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Skeleton tomou dano! Vida atual: " + currentHealth);
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Skeleton morreu!");
        Destroy(gameObject);
    }
}
