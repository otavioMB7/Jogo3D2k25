using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    [SerializeField] private Image barraDeVidaImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AlterarBarraDeVida(int currentHealth, int maxHealth){
        barraDeVidaImage.fillAmount = (float)currentHealth/maxHealth;
    }

}
