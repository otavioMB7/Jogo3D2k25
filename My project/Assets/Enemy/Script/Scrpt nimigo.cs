using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    public float attackDistance = 3.0f;
    public float tempoDano = 1.5f;
    private bool alreadyDealtDamage = false; // evita dano repetido durante o ataque

    private NavMeshAgent agent;
    private Animator anim;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            anim.SetBool("isWalking", true);
            anim.SetBool("attack", false);
            alreadyDealtDamage = false;
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("isWalking", false);

            if (Time.time > lastAttackTime + tempoDano)
            {
                anim.SetBool("attack", true);
                lastAttackTime = Time.time;

                if (!alreadyDealtDamage)
                {
                    DealDamage(); // Aplica o dano ao player
                    alreadyDealtDamage = true;
                }
            }
            else
            {
                anim.SetBool("attack", false);
                alreadyDealtDamage = false;
            }
        }
    }

    // Método que aplica o dano
    void DealDamage()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackDistance)
        {
            PlayerDano health = player.GetComponent<PlayerDano>();
            if (health != null)
            {
                health.TakeDamage(10); // Dano causado
            }
        }
    }
}