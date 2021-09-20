using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptsMenu : MonoBehaviour{
    
    public string nomeCenaJogar;
    public string nomeCenaDesconectar;

    public void mudarCenaJogar(){
        SceneManager.LoadScene(nomeCenaJogar);
    }
    
    public void Desconectar(){
        SceneManager.LoadScene(nomeCenaDesconectar);
    }

}
