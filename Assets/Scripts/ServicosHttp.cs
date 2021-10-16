
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
                Debug.Log($@"{(int) resposta.StatusCode} ({resposta.ReasonPhrase})");
                if (resposta.IsSuccessStatusCode){
                    string conteudoReposta = resposta.Content.ReadAsStringAsync().Result;
                    Debug.Log(conteudoReposta);
                    ReturnRequest<T> returnRequest = JsonUtility.FromJson<ReturnRequest<T>>(conteudoReposta);
                    Debug.Log($@"Status conexão servidor: {returnRequest.status}");
                    if(returnRequest.status.Equals("200", System.StringComparison.OrdinalIgnoreCase)) {
                        Debug.Log($@"Conectado");
                    }
                    return Task.FromResult<T>(returnRequest.data);
                }
            }
        }
        return null;
    }

    public static IEnumerator PublicaConteudoServidor(string endereco, T objeto) {
        if(!string.IsNullOrEmpty(endereco)
        && objeto != null) {
            string JSON = JsonUtility.ToJson(objeto);
            var request = new UnityWebRequest (endereco, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JSON);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            Debug.Log(request.responseCode);
        }
    }
}
