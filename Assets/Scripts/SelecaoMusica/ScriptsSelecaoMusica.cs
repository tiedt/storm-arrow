using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsSelecaoMusica : MonoBehaviour{    

    public void Comecar() {
        if(PerfilLogado.Instance.conectado)
            if((!String.IsNullOrEmpty(Musica.EstiloSelecionado))
            && (!String.IsNullOrEmpty(Musica.MusicaSelecionada))) {
                SceneManager.LoadScene(Constantes.Cenas.Palco);
            }                     
    }

    public void Voltar() {
        if(PerfilLogado.Instance.conectado)
            SceneManager.LoadScene(Constantes.Cenas.Menu);
    }

}
