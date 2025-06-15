using UnityEngine;
using UnityEngine.SceneManagement;

public class C : MonoBehaviour
{
    public GameObject[] Abas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // Libera o cursor
        Cursor.visible = true;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Sair(){
        Debug.Log("Sair do jogo");
        Application.Quit();
    }
    public void Voltar(){
        Abas[0].gameObject.SetActive(true);
        for(int i = 1;i<3;i++){
            Abas[i].gameObject.SetActive(false);
        }
    }
    public void Jogar(){
        Abas[1].gameObject.SetActive(true);
        Abas[0].gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("aaaa");

    }
    public void Controles(){
        Abas[2].gameObject.SetActive(true);
        Abas[0].gameObject.SetActive(false);
        
    }
}
