using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Musica : MonoBehaviour{
    
    private static float percentualVolume = 30F;

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

    public static IEnumerator OuvirMusica(AudioSource audioSource) {
        if(audioSource != null
        && !Musica.EstiloSelecionado.Equals("", System.StringComparison.OrdinalIgnoreCase)
        && !Musica.MusicaSelecionada.Equals("", System.StringComparison.OrdinalIgnoreCase)){
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
