using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        Skeleton enemy = other.GetComponent<Skeleton>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Inimigo atingido pela Hitbox!");
        }
    }
}
