using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptsMenu : MonoBehaviour{
    
    public Text lbperfil;
    public Text lbpontuacao;
    public GameObject MenuOpcoes;
    public Slider volumeMusica;
    private Text nivelUsuario;
    private AudioSource SonsUI;

    public void Start() {
        volumeMusica = Utilidades.FindObject<GameObject>("SliderVolumePrincipal").GetComponent<Slider>();
        nivelUsuario = GameObject.FindGameObjectWithTag("MenuNivel").GetComponent<Text>();
        SonsUI = Utilidades.FindObject<GameObject>("SonsUI").GetComponent<AudioSource>();
        if (PerfilLogado.Instance.conectado) {
            lbperfil.text = PerfilLogado.Instance.nome;
            lbpontuacao.text = "P "+ String.Format("{0:n0}", PerfilLogado.Instance.pontuacao_Total);
            int index = PerfilLogado.Instance.Configuracoes.IndexOf(new PerfilConfiguracoes(){ config = "VolumePrincipal", idPerfil = PerfilLogado.Instance.id });            
            Musica.DefinirVolumeMusica(SonsUI, float.Parse(PerfilLogado.Instance.Configuracoes[index].valor));
            volumeMusica.value = Musica.PercentualVolume;
            nivelUsuario.text = "N "+ String.Format("{0:n0}", PerfilLogado.Instance.nivel);
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
            volumeMusica.value = Musica.PercentualVolume;
            SonsUI.volume = Musica.PercentualVolume;
            MenuOpcoes.SetActive(true);
        }
    }

    public void ConfirmarOpcoes() {
        Musica.DefinirVolumeMusica(SonsUI, volumeMusica.value);
        PerfilConfiguracoes configuracao = new PerfilConfiguracoes(){
            idPerfil = PerfilLogado.Instance.id,
            config = "VolumePrincipal",
            valor = Convert.ToString(Musica.PercentualVolume)
        };
        PerfilLogado.Instance.AtualizaConfiguracaoPerfil(configuracao);
        StartCoroutine(ServicosHttp<PerfilConfiguracoes>.AtualizaConteudoServidor($@"{Enderecos.PerfilConfiguracoes}", configuracao));
        FecharOpcoes();
    }

    public void FecharOpcoes() {
        if(MenuOpcoes != null)
            MenuOpcoes.SetActive(false);
    }

    public void ExibirCreditosJogo(){

    }

}
