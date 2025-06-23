using UnityEngine;

public class AttackHitboxEnemy : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter inimigo hitbox com: " + other.name);
        if (other.CompareTag("Player"))
        {
            PlayerDano health = other.GetComponent<PlayerDano>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Player tomou dano do Skeleton via hitbox!");
            }
        }
    }
}
