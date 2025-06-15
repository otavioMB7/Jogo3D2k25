using UnityEngine;
using UnityEngine.SceneManagement;

public class C : MonoBehaviour
{
    [SerializeField]private string nomeLevel;

    [SerializeField]private GameObject painelMenuInicial;
    [SerializeField]private GameObject painelControles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Sair(){
        Debug.Log("Sair do jogo");
        Application.Quit();
    }

    public void Jogar(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void AbrirControles(){
        painelMenuInicial.SetActive(false);
        painelControles.SetActive(true);


    }
    public void FecharControles(){
        painelControles.SetActive(false);
        painelMenuInicial.SetActive(true);
        
    }
}
