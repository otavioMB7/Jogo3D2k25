using UnityEngine;

public class PlayerDano : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int maxHealth = 100;
    private int currentHealth;
    [SerializeField]private BarraDeVida BarraDeVida;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Vida inicial: " + currentHealth);
        BarraDeVida.AlterarBarraDeVida(currentHealth,maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player tomou dano! Vida atual: " + currentHealth);
        BarraDeVida.AlterarBarraDeVida(currentHealth,maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("O player morreu!");
        // Aqui você pode desativar o controle do player, mostrar tela de morte, etc.
    }
}
