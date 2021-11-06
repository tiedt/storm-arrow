using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public AudioSource ErrouSource;

    // declarar como constantes
    private Color corNormal = Color.white;
    private Color corSolicitarTecla = Color.blue;

    private int RandomAtual;
    private int RandomAnterior;

    private int pont = 0;
    private PontuacaoMusica pontuacaoMusica = null;

    void Start()
    {
        if (PerfilLogado.Instance.conectado)
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

            ErrouSource = GameObject.FindGameObjectWithTag("Errou").GetComponent<AudioSource>();

            pontuacaoMusica = ServicosHttp<PontuacaoMusica>.RetornaObjetoServidor(Enderecos.PontuacaoMusicas + $@"?idPerfil={PerfilLogado.Instance.id}&estilo={Musica.EstiloSelecionado}&musica={Musica.MusicaSelecionada}").Result;
            if (pontuacaoMusica is null)
            {
                pontuacaoMusica = new PontuacaoMusica()
                {
                    id = 0, // Auto-sequence
                    idPerfil = PerfilLogado.Instance.id,
                    estilo = Musica.EstiloSelecionado,
                    musica = Musica.MusicaSelecionada,
                    pontuacao = 0
                };
                StartCoroutine(ServicosHttp<PontuacaoMusica>.PublicaConteudoServidor(Enderecos.PontuacaoMusica, pontuacaoMusica));
            }
        }
    }

    void Update()
    {
        if (MusicaSource.clip != null && MusicaSource.clip.loadState == AudioDataLoadState.Loaded && !MusicaSource.isPlaying)
        {
            print("Parou de tocar");
            SceneManager.LoadScene(Constantes.Cenas.TelaResumo);
        }
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
            if (CorBotao(EsquerdaButton) == corSolicitarTecla) IncrementarPontuacao(); else DecrementarPontuacao();
            MudarCorBotao(EsquerdaButton, corNormal);
        }
        else if (Input.GetKeyDown((KeyCode.DownArrow)))
        {
            if (CorBotao(BaixoButton) == corSolicitarTecla) IncrementarPontuacao(); else DecrementarPontuacao();
            MudarCorBotao(BaixoButton, corNormal);
        }
        else if (Input.GetKeyDown((KeyCode.RightArrow)))
        {
            if (CorBotao(DireitaButton) == corSolicitarTecla) IncrementarPontuacao(); else DecrementarPontuacao();
            MudarCorBotao(DireitaButton, corNormal);
        }
        else if (Input.GetKeyDown((KeyCode.UpArrow)))
        {
            if (CorBotao(CimaButton) == corSolicitarTecla) IncrementarPontuacao(); else DecrementarPontuacao();
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

    private void IncrementarPontuacao()
    {
        LabelPontuacao.text = CalculaValorPontuacao(clickAnterior).ToString();
        clickAnterior = DateTime.Now;
    }

    private void DecrementarPontuacao()
    {
        ErrouSource.Play();
        pont -= 500;
        if (pont <= 0)
        {
            pont = 0;
        }
        LabelPontuacao.text = pont.ToString();
        clickAnterior = DateTime.Now;
    }

    private int CalculaValorPontuacao(DateTime ultimoClick)
    {
        var calc = DateTime.Now - ultimoClick;
        if (calc.TotalSeconds < 1)
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

    public void AtualizarPontuacaoTotalPerfil()
    {
        if (PerfilLogado.Instance.conectado)
        {
            PerfilLogado.Instance.pontuacao_Total += pont;
            Perfil perfil = new Perfil(PerfilLogado.Instance);
            StartCoroutine(ServicosHttp<Perfil>.AtualizaConteudoServidor(Enderecos.Perfil, perfil));
        }
    }

    public void AtualizaPontuacaoMusica()
    {
        if (PerfilLogado.Instance.conectado
        && !String.IsNullOrEmpty(Musica.EstiloSelecionado)
        && !String.IsNullOrEmpty(Musica.MusicaSelecionada)
        && pont > pontuacaoMusica.pontuacao)
        {
            pontuacaoMusica.pontuacao = pont;
            StartCoroutine(ServicosHttp<PontuacaoMusica>.AtualizaConteudoServidor(Enderecos.PontuacaoMusica, pontuacaoMusica));
        }
    }

}
