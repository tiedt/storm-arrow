using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour{
    
    public Dropdown DropDownEstilosMusicais;
    public Dropdown DropDownMusicas;
    public AudioSource PreMusica;

    void Start(){

        try{            
            // Zerando os comboboxes
            DropDownEstilosMusicais.options.Clear();
            DropDownMusicas.options.Clear();            

            string[] listaPastasDeEstilosMusicais = Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() +"\\Musicas");
            int indexEstiloSelecionado = -1;
            foreach(string pastaDeEstiloMusical in listaPastasDeEstilosMusicais) {
                DirectoryInfo nomePasta = new DirectoryInfo(pastaDeEstiloMusical.Trim());
                // Adiciona o estilo música, que é o nome da pasta física
                DropDownEstilosMusicais.options.Add(new Dropdown.OptionData(){ text = nomePasta.Name.Trim() });
                if (!string.IsNullOrEmpty(Musica.EstiloSelecionado)) {
                    indexEstiloSelecionado++;
                    if(Musica.EstiloSelecionado.Equals(nomePasta.Name.Trim(), System.StringComparison.OrdinalIgnoreCase)) {
                        Musica.EstiloSelecionado = "";
                    }
                }
            }

            // Evento que indica alteração no item do combobox
            DropDownEstilosMusicais.onValueChanged.AddListener(delegate{ 
                OnDropdownEstilosMusicaisItemChange();
            });            

            // Garantia de que não vai ocorrer problemas
            if(DropDownEstilosMusicais.options.Count > 0){                
            
                DropDownEstilosMusicais.value = indexEstiloSelecionado;

                Musica.EstiloSelecionado = DropDownEstilosMusicais.options[DropDownEstilosMusicais.value].text.Trim();

                CarregarMusicasDiretorioEstilo(Musica.EstiloSelecionado, !string.IsNullOrEmpty(Musica.MusicaSelecionada));
                
                DropDownMusicas.onValueChanged.AddListener(delegate{ 
                    OnDropdownMusicaisItemChange();
                });

                OnDropdownMusicaisItemChange();
            }
        }finally { 
            Musica.PararMusica(PreMusica, true);
            // Atualiza a visualização dos comboboxes
            DropDownEstilosMusicais.RefreshShownValue();
            DropDownEstilosMusicais.Hide();
        }
    }
    
    private void CarregarMusicasDiretorioEstilo(string Estilo, bool CarregarMusicaSalva = false) {

        try{
            DropDownMusicas.options.Clear();
            if(!CarregarMusicaSalva)
                Musica.MusicaSelecionada = "";

            string[] listaArquivosMusicas = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "\\Musicas\\" + Estilo);
            
            int indexMusicaSalva = -1;
            foreach (string arquivoMusica in listaArquivosMusicas) {
                // Só permite arquivos com nome terminado em MP3
                if (arquivoMusica.Trim().EndsWith("MP3", System.StringComparison.OrdinalIgnoreCase)){
                    string nomeMusica = Path.GetFileNameWithoutExtension(arquivoMusica);
                    // Adiciona a música, que é o nome do arquivo físico
                    DropDownMusicas.options.Add(new Dropdown.OptionData(){ text = nomeMusica});                    
                    if (!string.IsNullOrEmpty(Musica.MusicaSelecionada)){
                        indexMusicaSalva++;
                        if(nomeMusica.Equals(Musica.MusicaSelecionada, System.StringComparison.OrdinalIgnoreCase)) {
                            Musica.MusicaSelecionada = "";
                        }
                    }
                }
            }
            if(DropDownMusicas.options.Count > 0){
                DropDownMusicas.value = indexMusicaSalva;
                Musica.MusicaSelecionada = DropDownMusicas.options[DropDownMusicas.value].text.Trim();
            }
        } finally {
            Musica.PararMusica(PreMusica, true);
            DropDownMusicas.RefreshShownValue();
            DropDownMusicas.Hide();
        }
    }

    private void OnDropdownEstilosMusicaisItemChange() {
        try{
            Musica.EstiloSelecionado = DropDownEstilosMusicais.options[DropDownEstilosMusicais.value].text.Trim();

            CarregarMusicasDiretorioEstilo(Musica.EstiloSelecionado);
        } finally {
            Musica.PararMusica(PreMusica, true);
            DropDownEstilosMusicais.RefreshShownValue();
            DropDownEstilosMusicais.Hide();
        }
    }    

    private void OnDropdownMusicaisItemChange(){
        try{
            if(DropDownMusicas.options.Count > 0) {
                Musica.MusicaSelecionada = DropDownMusicas.options[DropDownMusicas.value].text.Trim();
                if(PerfilLogado.Instance.PontuacaoMusicas.IndexOf(new PontuacaoMusica(){ 
                        idPerfil = PerfilLogado.Instance.id, 
                        estilo = Musica.EstiloSelecionado, 
                        musica = Musica.MusicaSelecionada }) < 0) {
                    PontuacaoMusica novaPontuacaoMusica = new PontuacaoMusica() {
                        idPerfil = PerfilLogado.Instance.id,
                        estilo = Musica.EstiloSelecionado,
                        musica = Musica.MusicaSelecionada
                    };
                    PerfilLogado.Instance.PontuacaoMusicas.Add(novaPontuacaoMusica);
                    string enderecoipv4PontuacaoMusica = $@"{Enderecos.PontuacaoMusica}";
                    StartCoroutine(ServicosHttp<PontuacaoMusica>.PublicaConteudoServidor(enderecoipv4PontuacaoMusica, novaPontuacaoMusica));
                }
            }else
                Musica.MusicaSelecionada = "";            
        } finally {
            Musica.PararMusica(PreMusica, true);
            DropDownMusicas.RefreshShownValue();
            DropDownMusicas.Hide();
        }
    }

    

}
