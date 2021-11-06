using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptTelaResumo : MonoBehaviour
{
    public void Reiniciar()
    {

        SceneManager.LoadScene(Constantes.Cenas.Palco);
    }

    public void Sair()
    {
        SceneManager.LoadScene(Constantes.Cenas.TelaInicial);
    }

    public void Continuar()
    {
        SceneManager.LoadScene(Constantes.Cenas.SelecaoMusica);
    }
}