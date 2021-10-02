using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour{

    public Dropdown DropDownEstilosMusicais;
    public Dropdown DropDownMusicas;

    // Start is called before the first frame update
    void Start(){
        
        string[] listaPastasDeEstilosMusicais = Directory.GetDirectories(Application.dataPath +"\\Musicas");
        
        foreach(string pastaDeEstiloMusical in listaPastasDeEstilosMusicais) {
            DirectoryInfo nomePasta = new DirectoryInfo(pastaDeEstiloMusical.Trim());
            DropDownEstilosMusicais.options.Add(new Dropdown.OptionData(){ text = nomePasta.Name.Trim() });
        }

        DropDownEstilosMusicais.onValueChanged.AddListener(delegate{ 
            OnDropDownEstilosMusicaisItemChange();
        });

        if(DropDownEstilosMusicais.options.Count > 0){
            
            DropDownEstilosMusicais.RefreshShownValue();
            
            string[] listaArquivosMusicas = Directory.GetFiles(Application.dataPath + "\\Musicas\\" + DropDownEstilosMusicais.options[DropDownEstilosMusicais.value].text);

            foreach (string arquivoMusica in listaArquivosMusicas) {
                if (arquivoMusica.ToUpper().Trim().EndsWith("MP3")){
                    string nomeMusica = Path.GetFileNameWithoutExtension(arquivoMusica);
                    DropDownMusicas.options.Add(new Dropdown.OptionData(){ text = nomeMusica});
                }               
            }

            DropDownMusicas.RefreshShownValue();
        }
    }
    private void OnDropDownEstilosMusicaisItemChange() {
       
    }

}
