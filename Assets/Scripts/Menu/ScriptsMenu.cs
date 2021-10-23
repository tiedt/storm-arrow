using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptsMenu : MonoBehaviour{
    
    public Text lbperfil;
    public Text lbpontuacao;
    public GameObject MenuOpcoes;

    public void Start() {
        if (PerfilLogado.Instance.conectado) {
            lbperfil.text = PerfilLogado.Instance.nome;
            lbpontuacao.text = "P "+ Convert.ToString(PerfilLogado.Instance.pontuacao_Total);
        }
    }

    public void MudarCenaJogar(){
        if(PerfilLogado.Instance.conectado)
            SceneManager.LoadScene(Constantes.Cenas.SelecaoMusica);
    }
    
    public void Desconectar(){
        if(PerfilLogado.Instance.conectado)
            PerfilLogado.Instance.DesconectarPerfil();
        SceneManager.LoadScene(Constantes.Cenas.TelaInicial);
    }

    public void AbrirOpcoes() {
        if(MenuOpcoes != null) { 
            MenuOpcoes.SetActive(true);
            Slider sliderMenuOpcoes = GameObject.FindGameObjectWithTag("SliderMenuOpcoes").GetComponent<Slider>();
            sliderMenuOpcoes.value = Musica.PercentualVolume;
        }
    }

    public void ConfirmarOpcoes() {
        Slider sliderMenuOpcoes = GameObject.FindGameObjectWithTag("SliderMenuOpcoes").GetComponent<Slider>();
        Musica.DefinirVolumeMusica(null, sliderMenuOpcoes.value);
        PerfilConfiguracoes configuracao = new PerfilConfiguracoes(){
            idPerfil = PerfilLogado.Instance.id,
            config = "VolumePrincipal",
            valor = Convert.ToString(sliderMenuOpcoes.value)
        };
        PerfilLogado.Instance.AtualizaConfiguracaoPerfil(configuracao);
        StartCoroutine(ServicosHttp<PerfilConfiguracoes>.AtualizaConteudoServidor($@"{Enderecos.PerfilConfiguracoes}", configuracao));
        FecharOpcoes();
    }

    public void FecharOpcoes() {
        if(MenuOpcoes != null)
            MenuOpcoes.SetActive(false);
    }

}
