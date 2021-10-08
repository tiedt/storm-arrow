using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsPalco : MonoBehaviour
{



    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown((KeyCode.LeftArrow)))
        {
            print("Esquerda");
        }
        else if (Input.GetKeyDown((KeyCode.RightArrow)))
        {
            print("Direita");
        }
        else if (Input.GetKeyDown((KeyCode.DownArrow)))
        {
            print("Baixo");
        }
        else if (Input.GetKeyDown((KeyCode.UpArrow)))
        {
            print("Cima");
        }
    }

}
