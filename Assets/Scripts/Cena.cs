using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cena : MonoBehaviour
{
    
    public string nomeCena;

    public void mudarCena()
    {
        SceneManager.LoadScene(nomeCena);
    }
}
