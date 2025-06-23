using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;

private void OnTriggerEnter(Collider other)
{
    Debug.Log("OnTriggerEnter player hitbox com: " + other.name);

    if (other.CompareTag("Enemy"))
    {
        Debug.Log("Tem tag Enemy, tentando pegar Skeleton...");
        
        var fromSelf = other.GetComponent<Skeleton>();
        var fromParent = other.GetComponentInParent<Skeleton>();
        var fromRoot = other.transform.root.GetComponent<Skeleton>();

        Debug.Log($"fromSelf: {(fromSelf != null)}, fromParent: {(fromParent != null)}, fromRoot: {(fromRoot != null)}");

        Skeleton enemy = fromSelf ?? fromParent ?? fromRoot;
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Inimigo atingido pela Hitbox!");
        }
        else
        {
            Debug.LogWarning("Collider tem Tag Enemy, mas não achei Skeleton em lugar nenhum!");
        }
    }
}

}