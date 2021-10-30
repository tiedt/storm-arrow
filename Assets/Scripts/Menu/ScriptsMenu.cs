using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptsMenu : MonoBehaviour{
    
    public Text lbperfil;
    public Text lbpontuacao;
    public GameObject MenuOpcoes;
    public Slider volumeMusica;

    public void Start() {
        //volumeMusica = GameObject.FindGameObjectWithTag("SliderMenuOpcoes").GetComponent<Slider>();
        volumeMusica = FindObject<GameObject>("SliderVolumePrincipal").GetComponent<Slider>();
        //volumeMusica = GameObject.Find("SliderVolumePrincipal").GetComponent<Slider>();
        if (PerfilLogado.Instance.conectado) {
            lbperfil.text = PerfilLogado.Instance.nome;
            lbpontuacao.text = "P "+ Convert.ToString(PerfilLogado.Instance.pontuacao_Total);
            int index = PerfilLogado.Instance.Configuracoes.IndexOf(new PerfilConfiguracoes(){ config = "VolumePrincipal", idPerfil = PerfilLogado.Instance.id });
            volumeMusica.value = Convert.ToInt32(PerfilLogado.Instance.Configuracoes[index].valor);
        }
    }

    public GameObject FindObject<T>(string name) where T : UnityEngine.Object {
        T[] objects = Resources.FindObjectsOfTypeAll<T>() as T[];
        foreach(T obj in objects){
            if(obj.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return obj as GameObject;
        }
        return null;
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
