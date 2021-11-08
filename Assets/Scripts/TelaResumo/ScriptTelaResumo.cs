using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptTelaResumo : MonoBehaviour{

    private Text TituloResumo;
    private Text NomeJogador;
    private Text NivelJogador;
    private Text PontuacaoTotal;
    private Text MelhorPontuacaoMusica;
    private Text PontuacaoMusica;
    private Text TotalTeclas;
    private Text Acertos;
    private Text Erros;
    private Text PercentualAcertos;
    private Text PercentualErros;
    private List<Image> listaEstrelas = new List<Image>();
    private AudioSource Aplausos;

    private void Start() {
        NomeJogador = GameObject.FindGameObjectWithTag("TelaResumoNomeJogador").GetComponent<Text>();
        TituloResumo = GameObject.FindGameObjectWithTag("TituloResumo").GetComponent<Text>();
        PontuacaoTotal = GameObject.FindGameObjectWithTag("TelaResumoValorPontuacaoTotal").GetComponent<Text>();
        NivelJogador = GameObject.FindGameObjectWithTag("TelaResumoNivel").GetComponent<Text>();
        MelhorPontuacaoMusica = GameObject.FindGameObjectWithTag("TelaResumoMelhorPontuacaoMusica").GetComponent<Text>();
        PontuacaoMusica = GameObject.FindGameObjectWithTag("TelaResumoValorPontuacaoMusica").GetComponent<Text>();
        TotalTeclas = GameObject.FindGameObjectWithTag("TelaResumoValorTotalTeclas").GetComponent<Text>();
        Acertos = GameObject.FindGameObjectWithTag("TelaResumoValorAcertos").GetComponent<Text>();
        Erros = GameObject.FindGameObjectWithTag("TelaResumoValorErros").GetComponent<Text>();
        PercentualAcertos = GameObject.FindGameObjectWithTag("TelaResumoPercentualAcertos").GetComponent<Text>();
        PercentualErros = GameObject.FindGameObjectWithTag("TelaResumoPercentualErros").GetComponent<Text>();
        Aplausos = Utilidades.FindObject<GameObject>("Aplausos").GetComponent<AudioSource>();

        listaEstrelas.Add(Utilidades.FindObject<GameObject>("imgEstrela1").GetComponent<Image>());
        listaEstrelas.Add(Utilidades.FindObject<GameObject>("imgEstrela2").GetComponent<Image>());
        listaEstrelas.Add(Utilidades.FindObject<GameObject>("imgEstrela3").GetComponent<Image>());
        listaEstrelas.Add(Utilidades.FindObject<GameObject>("imgEstrela4").GetComponent<Image>());
        listaEstrelas.Add(Utilidades.FindObject<GameObject>("imgEstrela5").GetComponent<Image>());

        TituloResumo.text = $@"Resumo - {Musica.MusicaSelecionada}";
        NomeJogador.text = PerfilLogado.Instance.nome;
        NivelJogador.text = String.Format("{0:n0}", PerfilLogado.Instance.nivel);
        PontuacaoTotal.text = String.Format("{0:n0}", PerfilLogado.Instance.pontuacao_Total);
        int indexPontuacaoMusica = PerfilLogado.Instance.PontuacaoMusicas.IndexOf(new PontuacaoMusica(){
                idPerfil = PerfilLogado.Instance.id,
                estilo = Musica.EstiloSelecionado,
                musica = Musica.MusicaSelecionada
            });
        MelhorPontuacaoMusica.text = String.Format("{0:n0}", PerfilLogado.Instance.PontuacaoMusicas[indexPontuacaoMusica].pontuacao);
        PontuacaoMusica.text = String.Format("{0:n0}", ScriptsPalco.pont);
        TotalTeclas.text = String.Format("{0:n0}", ScriptsPalco.totalTeclas);
        Acertos.text = String.Format("{0:n0}", ScriptsPalco.totalAcertos);
        Erros.text = String.Format("{0:n0}", ScriptsPalco.totalErros);
        
        float percentualAcertos = 0.0F;
        float percentualErros = 0.0F;
        if(ScriptsPalco.totalTeclas > 0.0F) {
            percentualAcertos = (ScriptsPalco.totalAcertos * 100f) / ScriptsPalco.totalTeclas;
            percentualErros = (ScriptsPalco.totalErros * 100f) / ScriptsPalco.totalTeclas;
        }
        PercentualAcertos.text = $@"{String.Format("{0:0.00}", percentualAcertos)}%";
        PercentualErros.text = $@"{String.Format("{0:0.00}", percentualErros)}%";

        if(percentualAcertos <= 20.0F)
            listaEstrelas[1].color = new Color(1.0F, 1.0F, 1.0F, 18.0F / 100);
        if(percentualAcertos < 40.0F)
            listaEstrelas[2].color = new Color(1.0F, 1.0F, 1.0F, 18.0F / 100);
        if(percentualAcertos < 60.0F)
            listaEstrelas[3].color = new Color(1.0F, 1.0F, 1.0F, 18.0F / 100);
        if(percentualAcertos < 80.0F)
            listaEstrelas[4].color = new Color(1.0F, 1.0F, 1.0F, 18.0F / 100);
        else
            StartCoroutine(OuvirAplausos());
        StartCoroutine(GirarEstrelas());
    }

    public IEnumerator OuvirAplausos() {
        Aplausos.volume = 0.0F;
        Aplausos.Play();
        float percentualEntreEscala = (Musica.PercentualVolume / 50F) / 100F;
        // Aumenta gradativamente o som
        for(int i = 0; i < 50; i++) {
            Aplausos.volume += percentualEntreEscala;
            yield return new WaitForSeconds(0.13F);
        }
        // Diminui gradativamente o som
        for(int i = 50; i > 0; i++) {
            Aplausos.volume -= percentualEntreEscala;
            yield return new WaitForSeconds(0.1F);
        }
        Aplausos.Stop();
    }

    public void Reiniciar(){
        SceneManager.LoadScene(Constantes.Cenas.Palco);
    }

    public void Sair(){
        SceneManager.LoadScene(Constantes.Cenas.Menu);
    }

    public void Continuar(){
        SceneManager.LoadScene(Constantes.Cenas.SelecaoMusica);
    }

    public IEnumerator GirarEstrelas(){
        int i;
        while (true){
            yield return new WaitForSeconds(0.08F);
            for(i = 0; i < listaEstrelas.Count; i++)
                if(i % 2 == 0)
                    listaEstrelas[i].transform.Rotate(0.0f, 0.0f, -1.0f, Space.Self);
                else
                    listaEstrelas[i].transform.Rotate(0.0f, 0.0f, 1.0f, Space.Self);
        }
    }
}