
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public static class Servicos<T>

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
}
