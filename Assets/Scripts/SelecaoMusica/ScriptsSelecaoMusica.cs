using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptsSelecaoMusica : MonoBehaviour{

    public string nomeCenaPalco;
    public string nomeCenaMenu;

    public void Comecar() {
        if((!String.IsNullOrEmpty(Musica.EstiloSelecionado))
        && (!String.IsNullOrEmpty(Musica.MusicaSelecionada))) {
            SceneManager.LoadScene(nomeCenaPalco);
        }                     
    }

    public void Voltar() {
        SceneManager.LoadScene(nomeCenaMenu);
    }

}
