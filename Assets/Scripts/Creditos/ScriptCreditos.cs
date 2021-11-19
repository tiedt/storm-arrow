using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptCreditos : MonoBehaviour
{
    public void Sair()
    {
        if (PerfilLogado.Instance.conectado)
            SceneManager.LoadScene(Constantes.Cenas.Menu);
    }
}