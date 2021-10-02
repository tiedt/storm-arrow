using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour{
    
    public Dropdown DropDownEstilosMusicais;
    public Dropdown DropDownMusicas;

    public static string EstiloSelecionado = "";
    public static string MusicaSelecionada = "";

    // Start is called before the first frame update
    void Start(){

        try{            
            // Zerando os comboboxes
            DropDownEstilosMusicais.options.Clear();
            DropDownMusicas.options.Clear();
            
            string[] listaPastasDeEstilosMusicais = Directory.GetDirectories(Application.dataPath +"\\Musicas");
            int indexEstiloSelecionado = -1;
            foreach(string pastaDeEstiloMusical in listaPastasDeEstilosMusicais) {
                DirectoryInfo nomePasta = new DirectoryInfo(pastaDeEstiloMusical.Trim());
                // Adiciona o estilo música, que é o nome da pasta física
                DropDownEstilosMusicais.options.Add(new Dropdown.OptionData(){ text = nomePasta.Name.Trim() });
                if (!string.IsNullOrEmpty(EstiloSelecionado)) {
                    indexEstiloSelecionado++;
                    if(EstiloSelecionado.Equals(nomePasta.Name.Trim(), System.StringComparison.OrdinalIgnoreCase)) {
                        EstiloSelecionado = "";
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

                EstiloSelecionado = DropDownEstilosMusicais.options[DropDownEstilosMusicais.value].text.Trim();

                CarregarMusicasDiretorioEstilo(EstiloSelecionado, !string.IsNullOrEmpty(MusicaSelecionada));
                
                DropDownMusicas.onValueChanged.AddListener(delegate{ 
                    OnDropdownMusicaisItemChange();
                });

            }
        }finally { 
            // Atualiza a visualização dos comboboxes
            DropDownEstilosMusicais.RefreshShownValue();
        }
    }
    
    private void CarregarMusicasDiretorioEstilo(string Estilo, bool CarregarMusicaSalva = false) {

        try{
            DropDownMusicas.options.Clear();
            if(!CarregarMusicaSalva)
                MusicaSelecionada = "";

            string[] listaArquivosMusicas = Directory.GetFiles(Application.dataPath + "\\Musicas\\" + Estilo);
            
            int indexMusicaSalva = -1;
            foreach (string arquivoMusica in listaArquivosMusicas) {
                // Só permite arquivos com nome terminado em MP3
                if (arquivoMusica.Trim().EndsWith("MP3", System.StringComparison.OrdinalIgnoreCase)){
                    string nomeMusica = Path.GetFileNameWithoutExtension(arquivoMusica);
                    // Adiciona a música, que é o nome do arquivo físico
                    DropDownMusicas.options.Add(new Dropdown.OptionData(){ text = nomeMusica});                    
                    if (!string.IsNullOrEmpty(MusicaSelecionada)){
                        indexMusicaSalva++;
                        if(nomeMusica.Equals(MusicaSelecionada, System.StringComparison.OrdinalIgnoreCase)) {
                            MusicaSelecionada = "";
                        }
                    }
                }
            }
            if(DropDownMusicas.options.Count > 0){
                DropDownMusicas.value = indexMusicaSalva;
                MusicaSelecionada = DropDownMusicas.options[DropDownMusicas.value].text.Trim();
            }
        } finally {
            DropDownMusicas.RefreshShownValue();
        }
    }

    private void OnDropdownEstilosMusicaisItemChange() {
        try{
            EstiloSelecionado = DropDownEstilosMusicais.options[DropDownEstilosMusicais.value].text.Trim();

            CarregarMusicasDiretorioEstilo(EstiloSelecionado);
        } finally {
            DropDownEstilosMusicais.RefreshShownValue();
        }
    }    

    private void OnDropdownMusicaisItemChange(){
        try{
            if(DropDownMusicas.options.Count > 0)
                MusicaSelecionada = DropDownMusicas.options[DropDownMusicas.value].text.Trim();
            else
                MusicaSelecionada = "";
        } finally {
            DropDownMusicas.RefreshShownValue();
        }
    }

}
