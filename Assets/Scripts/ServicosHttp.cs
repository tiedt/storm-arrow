
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ServicosHttp<T>

{
    public static Task<T> RetornaObjetoServidor(string endereco) {
        using (HttpClient httpClient = new HttpClient()) {
            using(HttpResponseMessage resposta = httpClient.GetAsync(endereco).Result) {
                if (resposta.IsSuccessStatusCode){
                    string conteudoReposta = resposta.Content.ReadAsStringAsync().Result;
                    ReturnRequest<T> returnRequest = JsonUtility.FromJson<ReturnRequest<T>>(conteudoReposta);
                    if(returnRequest.data != null)
                        return Task.FromResult<T>(returnRequest.data);
                }
            }
        }
        return Task.FromResult<T>(default(T));
    }

    public static IEnumerator PublicaConteudoServidor(string endereco, T objeto) {
        if(!string.IsNullOrEmpty(endereco)
        && objeto != null) {
            string JSON = JsonUtility.ToJson(objeto);
            var requisicao = new UnityWebRequest (endereco, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JSON);
            requisicao.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            requisicao.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            requisicao.SetRequestHeader("Content-Type", "application/json");
            yield return requisicao.SendWebRequest();

            //Debug.Log(request.responseCode);
        }
    }

    public static IEnumerator AtualizaConteudoServidor(string endereco, T objeto) {
        if(!string.IsNullOrEmpty(endereco)
        && objeto != null) {
            string JSON = JsonUtility.ToJson(objeto);
            UnityWebRequest requisicao = UnityWebRequest.Put(endereco, JSON);
            requisicao.SetRequestHeader("Content-Type", "application/json");
            yield return requisicao.SendWebRequest();
        }
    }
}
