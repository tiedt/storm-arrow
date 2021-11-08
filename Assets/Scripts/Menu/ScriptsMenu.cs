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

    public void Start() {
        volumeMusica = Utilidades.FindObject<GameObject>("SliderVolumePrincipal").GetComponent<Slider>();
        nivelUsuario = GameObject.FindGameObjectWithTag("MenuNivel").GetComponent<Text>();
        if (PerfilLogado.Instance.conectado) {
            lbperfil.text = PerfilLogado.Instance.nome;
            lbpontuacao.text = "P "+ String.Format("{0:n0}", PerfilLogado.Instance.pontuacao_Total);
            int index = PerfilLogado.Instance.Configuracoes.IndexOf(new PerfilConfiguracoes(){ config = "VolumePrincipal", idPerfil = PerfilLogado.Instance.id });            
            volumeMusica.value = float.Parse(PerfilLogado.Instance.Configuracoes[index].valor);
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
            MenuOpcoes.SetActive(true);
            volumeMusica.value = Musica.PercentualVolume;
        }
    }

    public void ConfirmarOpcoes() {
        Musica.DefinirVolumeMusica(null, volumeMusica.value);
        PerfilConfiguracoes configuracao = new PerfilConfiguracoes(){
            idPerfil = PerfilLogado.Instance.id,
            config = "VolumePrincipal",
            valor = Convert.ToString(volumeMusica.value)
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
