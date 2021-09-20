using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsTelaInicial : MonoBehaviour{

    public string cenaLogar;
    public void Logar() {
        SceneManager.LoadScene(cenaLogar);
    }
    
    public void Quitar() {
        #if UNITY_EDITOR
            print("Fechando...");
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif
    }
}
