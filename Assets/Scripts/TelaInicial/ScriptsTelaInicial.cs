using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptsTelaInicial : MonoBehaviour{

    public string cenaLogar;
    public InputField edNomeNovoPerfil;    
    private Dropdown cbPerfis;

    private void Start() {
        try {
            edNomeNovoPerfil.gameObject.SetActive(false);
            cbPerfis = GameObject.FindGameObjectWithTag("cbPerfis").GetComponent<Dropdown>();
            cbPerfis.options.Clear();

            List<Perfil> perfis = ServicosHttp<List<Perfil>>.RetornaObjetoServidor(Enderecos.Perfis +"?macAddress="+ ServicosUtils.RetornaMelhorEnderecoMac()).Result;
            foreach(Perfil perfil in perfis) {
                cbPerfis.options.Add(new Dropdown.OptionData(){ text = perfil.nome.Trim()});
            }
        } finally {
            cbPerfis.RefreshShownValue();
        }
    }

    public void Logar() {
        SceneManager.LoadScene(cenaLogar);
    }
    
    public void Quitar() {
        #if UNITY_EDITOR
            print("Fechando...");
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif
    }

    public void CriarNovoPerfil() {
        edNomeNovoPerfil.gameObject.SetActive(true);
        edNomeNovoPerfil.onEndEdit.AddListener(delegate { EdNomeNovoPerfilOnChange(); });   
    }

    private void EdNomeNovoPerfilOnChange() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log("Confirmou");
            if (!edNomeNovoPerfil.text.Trim().Equals("", System.StringComparison.OrdinalIgnoreCase)) {
                
                bool perfilJaCadastrado = false;
                foreach(Dropdown.OptionData itemCombo in cbPerfis.options) {
                    perfilJaCadastrado = perfilJaCadastrado
                                      || itemCombo.text.Trim().Equals(edNomeNovoPerfil.text.Trim(), System.StringComparison.OrdinalIgnoreCase);
                }
                if (!perfilJaCadastrado) {
                    Perfil novoPerfil = new Perfil();
                    novoPerfil.id = 0; // Auto-sequence
                    novoPerfil.nome = edNomeNovoPerfil.text.Trim();
                    novoPerfil.endereco_Mac = ServicosUtils.RetornaMelhorEnderecoMac();
                    novoPerfil.pontuacao_Total = 0;
                    StartCoroutine(ServicosHttp<Perfil>.PublicaConteudoServidor(Enderecos.Perfil, novoPerfil));

                    edNomeNovoPerfil.gameObject.SetActive(false);
                    edNomeNovoPerfil.Select();
                    edNomeNovoPerfil.ActivateInputField();

                    cbPerfis.options.Add(new Dropdown.OptionData(){ text = edNomeNovoPerfil.text.Trim()});
                }
            }
        }
    }
}
