using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Musica : MonoBehaviour{
    
    private static float percentualVolume = 100F;

    public static string EstiloSelecionado = "";
    public static string MusicaSelecionada = "";
    public Sprite spriteOuvirMusica;
    public Sprite spriteNaoOuvirMusica;

    public Musica musica;
    public static Musica Instance { get; private set; }

    private void Awake() {
        if (Instance == null) 
            Instance = this;
        else
            Destroy(gameObject);
        // Cache references to all desired variables
        musica = FindObjectOfType<Musica>();
    }

    private void Start() {
        string enderecoipv4 = $@"{Enderecos.Perfis}?macAddress=74-D0-2B-9E-1F-63";
        using (HttpClient httpClient = new HttpClient()) {
            using(HttpResponseMessage resposta = httpClient.GetAsync(enderecoipv4).Result) {
                Debug.Log($@"{(int) resposta.StatusCode} ({resposta.ReasonPhrase})");
                if (resposta.IsSuccessStatusCode){
                    string conteudoReposta = resposta.Content.ReadAsStringAsync().Result;
                    Debug.Log(conteudoReposta);
                    ReturnRequest<List<Perfil>> JSON = JsonUtility.FromJson<ReturnRequest<List<Perfil>>>(conteudoReposta);

                    Debug.Log($@"Status conexão servidor: {JSON.status}");
                    if(JSON.status.Equals("200", System.StringComparison.OrdinalIgnoreCase)) {
                        Debug.Log($@"Conectado");
                    }
                }
            }
        }
    }

    public static IEnumerator OuvirMusica(AudioSource audioSource) {
        if(audioSource != null){
            PararMusica(audioSource);
            if((audioSource.clip is null)
             ||(!audioSource.clip.name.Equals(Musica.MusicaSelecionada + ".mp3", System.StringComparison.OrdinalIgnoreCase))) {
                UnityWebRequest recurso = UnityWebRequestMultimedia.GetAudioClip(Application.dataPath + "\\Musicas\\" + Musica.EstiloSelecionado +"\\"+ Musica.MusicaSelecionada +".mp3", AudioType.MPEG);
                yield return recurso.SendWebRequest();
                audioSource.clip = DownloadHandlerAudioClip.GetContent(recurso);
                audioSource.name = Musica.MusicaSelecionada +".mp3";   
            }
            audioSource.volume = Musica.percentualVolume / 100F;
            audioSource.loop = false;

            audioSource.Play();

            GameObject gameObject = GameObject.FindGameObjectWithTag("btnOuvirMusica");
            if(gameObject != null){
                Button btnOuvirMusica = gameObject.GetComponent<Button>();
                btnOuvirMusica.image.sprite = Musica.Instance.spriteNaoOuvirMusica;
            }
        }
    }

    public static void PausarMusica(AudioSource audioSource) {
        if(audioSource != null){
            if(audioSource.isPlaying)
                audioSource.Pause();
            GameObject gameObject = GameObject.FindGameObjectWithTag("btnOuvirMusica");
            if(gameObject != null){
                Button btnOuvirMusica = gameObject.GetComponent<Button>();
                btnOuvirMusica.image.sprite = Musica.Instance.spriteOuvirMusica;
            }
        }
    }

    public static void ContinuarMusica(AudioSource audioSource) {
        if(audioSource != null) {
            if(!audioSource.isPlaying)
                audioSource.UnPause();
            GameObject gameObject = GameObject.FindGameObjectWithTag("btnOuvirMusica");
            if(gameObject != null){
                Button btnOuvirMusica = gameObject.GetComponent<Button>();
                btnOuvirMusica.image.sprite = Musica.Instance.spriteNaoOuvirMusica;
            }
        }
    }

    public static void PararMusica(AudioSource audioSource, bool setNullToClip = false) {
        if(audioSource != null){
            if(audioSource.isPlaying)
                audioSource.Stop();
            if(setNullToClip)
                audioSource.clip = null;
            GameObject gameObject = GameObject.FindGameObjectWithTag("btnOuvirMusica");
            if(gameObject != null){
                Button btnOuvirMusica = gameObject.GetComponent<Button>();
                btnOuvirMusica.image.sprite = Musica.Instance.spriteOuvirMusica;
            }
        }
    }

    public static void ReiniciarMusica(AudioSource audioSource) {
        if(audioSource != null){
            PararMusica(audioSource);
            audioSource.Play();
            GameObject gameObject = GameObject.FindGameObjectWithTag("btnOuvirMusica");
            if(gameObject != null){
                Button btnOuvirMusica = gameObject.GetComponent<Button>();
                btnOuvirMusica.image.sprite = Musica.Instance.spriteOuvirMusica;
            }
        }
    }

    public static void DefinirVolumeMusica(AudioSource audioSource, float percentual = 100F) {
        if(audioSource != null) {
            percentualVolume = percentual;
            audioSource.volume = percentualVolume / 100F;
        }
    }

    public static void ReproduzirParar(AudioSource audioSource) {
        if(audioSource != null)
            if(audioSource.clip is null)
                Musica.Instance.StartCoroutine(Musica.OuvirMusica(audioSource));
            else{
                if(audioSource.isPlaying)
                    PausarMusica(audioSource);
                else
                    ContinuarMusica(audioSource);
            }
    }

}
