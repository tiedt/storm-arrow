using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptsMenu : MonoBehaviour{
    
    public Text lbperfil;
    public Text lbpontuacao;

    public void Start() {
        if (PerfilLogado.Instance.conectado) {
            lbperfil.text = PerfilLogado.Instance.nome;
            lbpontuacao.text = "P "+ Convert.ToString(PerfilLogado.Instance.pontuacao_Total);
        }
    }

    public void MudarCenaJogar(){
        if(PerfilLogado.Instance.conectado)
            SceneManager.LoadScene(Constantes.Cenas.SelecaoMusica);
    }
    
    public void Desconectar(){
        if(PerfilLogado.Instance.conectado)
            PerfilLogado.Instance.DesconectarPerfil();
        SceneManager.LoadScene(Constantes.Cenas.TelaInicial);
    }

}
