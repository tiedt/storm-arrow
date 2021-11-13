using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsSelecaoMusica : MonoBehaviour{

    private AudioSource PreMusica;
    private AudioSource SonsUI;
    public void Start() {
        PreMusica = Utilidades.FindObject<GameObject>("PreMusica").GetComponent<AudioSource>();
        SonsUI = Utilidades.FindObject<GameObject>("SonsUI").GetComponent<AudioSource>();
        PreMusica.volume = Musica.PercentualVolume / 100F;
        SonsUI.volume = Musica.PercentualVolume / 100F;
    }

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
