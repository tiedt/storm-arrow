using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptsPalco : MonoBehaviour
{
    public Button EsquerdaButton;
    public Button BaixoButton;
    public Button DireitaButton;
    public Button CimaButton;

    // declarar como constantes
    private Color corNormal = Color.white;
    private Color corSolicitarTecla = Color.blue;

    private int RandomAtual;
    private int RandomAnterior;

    void Start()
    {
        EsquerdaButton = GameObject.FindGameObjectWithTag("EsquerdaButton").GetComponent<Button>();
        MudarCorBotao(EsquerdaButton, corNormal);

        BaixoButton = GameObject.FindGameObjectWithTag("BaixoButton").GetComponent<Button>();
        MudarCorBotao(BaixoButton, corNormal);

        DireitaButton = GameObject.FindGameObjectWithTag("DireitaButton").GetComponent<Button>();
        MudarCorBotao(DireitaButton, corNormal);

        CimaButton = GameObject.FindGameObjectWithTag("CimaButton").GetComponent<Button>();
        MudarCorBotao(CimaButton, corNormal);

        RandomAtual = -1;
        RandomAnterior = -1;
    }

    void Update()
    {
        if ((CorBotao(EsquerdaButton) == corNormal) &&
               (CorBotao(BaixoButton) == corNormal) &&
               (CorBotao(DireitaButton) == corNormal) &&
               (CorBotao(CimaButton) == corNormal))
        {
            while (RandomAtual == RandomAnterior)
            {
                RandomAtual = Random.Range(0, 4);
            }
            RandomAnterior = RandomAtual;
            switch (RandomAtual)
            {
                case 0:
                    MudarCorBotao(EsquerdaButton, corSolicitarTecla);
                    break;
                case 1:
                    MudarCorBotao(BaixoButton, corSolicitarTecla);
                    break;
                case 2:
                    MudarCorBotao(DireitaButton, corSolicitarTecla);
                    break;
                case 3:
                    MudarCorBotao(CimaButton, corSolicitarTecla);
                    break;
                default:
                    break;
            }
        }
        if (Input.GetKeyDown((KeyCode.LeftArrow)))
        {
            print("Esquerda");
            MudarCorBotao(EsquerdaButton, corNormal);
        }
        else if (Input.GetKeyDown((KeyCode.DownArrow)))
        {
            print("Baixo");
            MudarCorBotao(BaixoButton, corNormal);
        }
        else if (Input.GetKeyDown((KeyCode.RightArrow)))
        {
            print("Direita");
            MudarCorBotao(DireitaButton, corNormal);
        }
        else if (Input.GetKeyDown((KeyCode.UpArrow)))
        {
            print("Cima");
            MudarCorBotao(CimaButton, corNormal);
        }
    }

    private Color CorBotao(Button botao)
    {
        var colors = botao.colors;
        return colors.normalColor;
    }

    private void MudarCorBotao(Button botao, Color cor)
    {
        var colors = botao.colors;
        colors.normalColor = cor;
        botao.colors = colors;
    }

}
