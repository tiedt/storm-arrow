using System;
using UnityEngine;
using UnityEngine.UI;

public class ScriptsPalco : MonoBehaviour
{
    public Button EsquerdaButton;
    public Button BaixoButton;
    public Button DireitaButton;
    public Button CimaButton;
    public AudioSource MusicaSource;
    public Text LabelPontuacao;
    public DateTime clickAnterior = DateTime.Now;

    // declarar como constantes
    private Color corNormal = Color.white;
    private Color corSolicitarTecla = Color.blue;

    private int RandomAtual;
    private int RandomAnterior;

    private int pont = 0;

    void Start()
    {
        LabelPontuacao = GameObject.FindGameObjectWithTag("Pontuacao").GetComponent<Text>();

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

        MusicaSource = GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>();
        StartCoroutine(Musica.OuvirMusica(MusicaSource));
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
                RandomAtual = UnityEngine.Random.Range(0, 4);
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
        IncrementarPontuacao(botao);
        var colors = botao.colors;
        colors.normalColor = cor;
        botao.colors = colors;
    }

    private void IncrementarPontuacao(Button botao)
    {
        if (CorBotao(botao) == corSolicitarTecla)
        {
            LabelPontuacao.text = CalculaValorPontuacao(clickAnterior).ToString();
            clickAnterior = DateTime.Now;
        }
    }

    private int CalculaValorPontuacao(DateTime ultimoClick)
    {
        var calc = DateTime.Now - ultimoClick;
        if(calc.TotalSeconds < 1)
        {
            return pont += 1000;
        }
        else if (calc.TotalSeconds < 2 && calc.TotalSeconds > 1)
        {
            return pont += 900;
        }
        else if (calc.TotalSeconds < 3 && calc.TotalSeconds > 2)
        {
            return pont += 800;
        }
        else if (calc.TotalSeconds < 4 && calc.TotalSeconds > 3)
        {
            return pont += 700;
        }
        return pont += 500;
    }

}
