using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sair : MonoBehaviour
{
    public void Quitar()
    {
#if UNITY_EDITOR
        print("Fechando...");
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit ();
#endif
    }
}
