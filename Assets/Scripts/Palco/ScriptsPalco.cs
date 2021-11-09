using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptsPalco : MonoBehaviour{
    
    private Button EsquerdaButton;
    private Button BaixoButton;
    private Button DireitaButton;
    private Button CimaButton;
    private AudioSource MusicaSource;
    private Text LabelPontuacao;
    private DateTime clickAnterior = DateTime.Now;
    private AudioSource ErrouSource;
    private GameObject JanelaOpcoes;
    private Slider VolumePrincipal;
    private AudioSource Acertou;

    // declarar como constantes
    private readonly Color corNormal = Color.white;
    private readonly Color corSolicitarTecla = new Color(115, 86, 86, 255);

    private int RandomAtual;
    private int RandomAnterior;

    public static int pont = 0;
    private PontuacaoMusica pontuacaoMusica = null;
    public static int totalTeclas = 0;
    public static int totalAcertos = 0;
    public static int totalErros = 0;
    private bool finalizouMusica = false;

    void Start(){

        if (PerfilLogado.Instance.conectado){

            LabelPontuacao = GameObject.FindGameObjectWithTag("Pontuacao").GetComponent<Text>();
            EsquerdaButton = GameObject.FindGameObjectWithTag("EsquerdaButton").GetComponent<Button>();
            BaixoButton = GameObject.FindGameObjectWithTag("BaixoButton").GetComponent<Button>();
            DireitaButton = GameObject.FindGameObjectWithTag("DireitaButton").GetComponent<Button>();
            CimaButton = GameObject.FindGameObjectWithTag("CimaButton").GetComponent<Button>();
            MusicaSource = GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>();
            ErrouSource = GameObject.FindGameObjectWithTag("Errou").GetComponent<AudioSource>();
            JanelaOpcoes = Utilidades.FindObject<GameObject>("JanelaOpcoes");
            VolumePrincipal = Utilidades.FindObject<GameObject>("SliderVolumePrincipal").GetComponent<Slider>();
            Acertou = Utilidades.FindObject<GameObject>("Acertou").GetComponent<AudioSource>();

            pont = 0;
            RandomAtual = -1;
            RandomAnterior = -1;
            finalizouMusica = false;
            totalTeclas = 0;
            totalAcertos = 0;
            totalErros = 0;

            StartCoroutine(Musica.OuvirMusica(MusicaSource));
            int indexPontuacaoMusica = PerfilLogado.Instance.PontuacaoMusicas.IndexOf(new PontuacaoMusica(){
                    idPerfil = PerfilLogado.Instance.id,
                    estilo = Musica.EstiloSelecionado,
                    musica = Musica.MusicaSelecionada
                });
            pontuacaoMusica = PerfilLogado.Instance.PontuacaoMusicas[indexPontuacaoMusica];
            VolumePrincipal.value = Musica.PercentualVolume;
            ErrouSource.volume = Musica.PercentualVolume / 100F;
            Acertou.volume = (Musica.PercentualVolume * 0.4F) / 100F;
        }
    }

    void Update(){
        if (!finalizouMusica) {
            if (!JanelaOpcoes.activeSelf) {
                if (MusicaSource.clip != null
                &&  MusicaSource.time >= MusicaSource.clip.length){
                    finalizouMusica = true;
                    StartCoroutine(FimReproducaoMusica());
                }                
                if ((CorBotao(EsquerdaButton) == corNormal) &&
                        (CorBotao(BaixoButton) == corNormal) &&
                        (CorBotao(DireitaButton) == corNormal) &&
                        (CorBotao(CimaButton) == corNormal)){
                    while (RandomAtual == RandomAnterior){
                        RandomAtual = UnityEngine.Random.Range(0, 4);
                    }
                    RandomAnterior = RandomAtual;
                    switch (RandomAtual){
                        case 0: MudarCorBotao(EsquerdaButton, corSolicitarTecla); break;
                        case 1: MudarCorBotao(BaixoButton, corSolicitarTecla); break;
                        case 2: MudarCorBotao(DireitaButton, corSolicitarTecla); break;
                        case 3: MudarCorBotao(CimaButton, corSolicitarTecla); break;
                    }
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                //if (Input.GetKeyDown(KeyCode.LeftArrow)){
                    if (CorBotao(EsquerdaButton) == corSolicitarTecla) 
                        IncrementarPontuacao(); 
                    else 
                        DecrementarPontuacao();
                    MudarCorBotao(EsquerdaButton, corNormal);
                    Input.ResetInputAxes();
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow)){
                    if (CorBotao(BaixoButton) == corSolicitarTecla) 
                        IncrementarPontuacao(); 
                    else 
                        DecrementarPontuacao();
                    MudarCorBotao(BaixoButton, corNormal);
                    Input.ResetInputAxes();
                }
                else if (Input.GetKeyUp(KeyCode.RightArrow)){
                    if (CorBotao(DireitaButton) == corSolicitarTecla) 
                        IncrementarPontuacao(); 
                    else 
                        DecrementarPontuacao();
                    MudarCorBotao(DireitaButton, corNormal);
                    Input.ResetInputAxes();
                }
                else if (Input.GetKeyUp(KeyCode.UpArrow)){
                    if (CorBotao(CimaButton) == corSolicitarTecla) 
                        IncrementarPontuacao(); 
                    else 
                        DecrementarPontuacao();
                    MudarCorBotao(CimaButton, corNormal);
                    Input.ResetInputAxes();
                }
                if(CimaButton.colors.normalColor == corSolicitarTecla) {
                    CimaButton.Select(); // SetFocus
                } else {
                    if(BaixoButton.colors.normalColor == corSolicitarTecla) {
                        BaixoButton.Select(); // SetFocus
                    } else {
                        if(EsquerdaButton.colors.normalColor == corSolicitarTecla) {
                            EsquerdaButton.Select(); // SetFocus
                        } else {
                            if(DireitaButton.colors.normalColor == corSolicitarTecla) {
                                DireitaButton.Select(); // SetTocus
                            }
                        }
                    }
                }                    
            }
            if (Input.GetKeyUp(KeyCode.Escape)){
                if(!JanelaOpcoes.activeSelf)
                    AbreJanelaOpcoes();
            }
        }
    }

    private void AbreJanelaOpcoes() {
        Musica.PausarMusica(MusicaSource);
        JanelaOpcoes.SetActive(true);
    }

    public void ConfirmarConfiguracoes() {
        Musica.DefinirVolumeMusica(MusicaSource, VolumePrincipal.value);
        ErrouSource.volume = Musica.PercentualVolume / 100F;
        Acertou.volume = (Musica.PercentualVolume * 0.4F) / 100F;
        PerfilConfiguracoes configuracao = new PerfilConfiguracoes(){
            idPerfil = PerfilLogado.Instance.id,
            config = "VolumePrincipal",
            valor = Convert.ToString(VolumePrincipal.value)
        };
        PerfilLogado.Instance.AtualizaConfiguracaoPerfil(configuracao);
        StartCoroutine(ServicosHttp<PerfilConfiguracoes>.AtualizaConteudoServidor($@"{Enderecos.PerfilConfiguracoes}", configuracao));
        FechaJanelaOpcoes();
    }

    public void FechaJanelaOpcoes() {
        JanelaOpcoes.SetActive(false);
        Musica.ContinuarMusica(MusicaSource);
    }

    private Color CorBotao(Button botao){
        var colors = botao.colors;
        return colors.normalColor;
    }

    private void ContinuarMusica() {
        Musica.ContinuarMusica(MusicaSource);
        Musica.PararMusica(ErrouSource);
    }

    private void MudarCorBotao(Button botao, Color cor){
        var colors = botao.colors;
        colors.normalColor = cor;
        botao.colors = colors;
    }

    private void IncrementarPontuacao(){
        Acertou.Play();
        LabelPontuacao.text = CalculaValorPontuacao(clickAnterior).ToString();
        clickAnterior = DateTime.Now;
        totalAcertos++;
        totalTeclas++;
    }

    private void DecrementarPontuacao(){
        ErrouSource.Play();
        pont -= 500;
        if (pont <= 0){
            pont = 0;
        }
        LabelPontuacao.text = pont.ToString();
        clickAnterior = DateTime.Now;
        totalErros++;
        totalTeclas++;
        Musica.PausarMusica(MusicaSource);
        Invoke("ContinuarMusica", 0.30f);
    }

    private int CalculaValorPontuacao(DateTime ultimoClick){
        var calc = DateTime.Now - ultimoClick;
        if (calc.TotalSeconds < 1){
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


    private IEnumerator FimReproducaoMusica() {
        yield return new WaitForSeconds(2);
        AtualizarPontuacaoTotalPerfil();
        AtualizaPontuacaoMusica();
        SceneManager.LoadScene(Constantes.Cenas.TelaResumo);
    }

    public void AtualizarPontuacaoTotalPerfil(){
        if (PerfilLogado.Instance.conectado){
            PerfilLogado.Instance.pontuacao_Total += pont;
            Perfil perfil = new Perfil(PerfilLogado.Instance);
            StartCoroutine(ServicosHttp<Perfil>.AtualizaConteudoServidor(Enderecos.Perfil, perfil));
        }
    }

    public void AtualizaPontuacaoMusica(){
        if (PerfilLogado.Instance.conectado
        && !String.IsNullOrEmpty(Musica.EstiloSelecionado)
        && !String.IsNullOrEmpty(Musica.MusicaSelecionada)
        && pont > pontuacaoMusica.pontuacao){
            pontuacaoMusica.pontuacao = pont;
            StartCoroutine(ServicosHttp<PontuacaoMusica>.AtualizaConteudoServidor(Enderecos.PontuacaoMusica, pontuacaoMusica));
        }
    }

}
