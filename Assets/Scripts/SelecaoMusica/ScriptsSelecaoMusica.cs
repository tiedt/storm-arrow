using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsSelecaoMusica : MonoBehaviour{

    public string nomeCenaPalco;
    public string nomeCenaMenu;

    public static string selectMusica;
    public void Comecar() {
        SceneManager.LoadScene(nomeCenaPalco);
    }

    public void Voltar() {
        SceneManager.LoadScene(nomeCenaMenu);
    }

    public void selecionarMusica() {
        selectMusica = "Bandolero";
    }

}
