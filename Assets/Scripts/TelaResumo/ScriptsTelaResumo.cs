using UnityEngine;
using UnityEngine.SceneManagement;


public class ScriptsTelaResumo : MonoBehaviour
{

    void Start()
    {
      Constantes.Cenas.Palco.Pontu
        
    }

    public void Reiniciar()
    {
        //if (PerfilLogado.Instance.conectado)
        SceneManager.LoadScene(Constantes.Cenas.Palco);
    }

    public void Sair()
    {
        // if (PerfilLogado.Instance.conectado)
        //PerfilLogado.Instance.DesconectarPerfil();
        SceneManager.LoadScene(Constantes.Cenas.TelaInicial);
    }

    public void Continuar()
    {
        // if (PerfilLogado.Instance.conectado)
        //PerfilLogado.Instance.DesconectarPerfil();
        SceneManager.LoadScene(Constantes.Cenas.SelecaoMusica);
    }


}
