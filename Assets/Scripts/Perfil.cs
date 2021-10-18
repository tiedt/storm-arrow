using System;

[System.Serializable]
public class Perfil{

    public int id;
    public string endereco_Mac;
    public string nome;
    public int pontuacao_Total;
    
    public Perfil() { }

    public Perfil(PerfilLogado perfilLogado) {
        if(perfilLogado != null) {
            id = perfilLogado.id;
            endereco_Mac = perfilLogado.endereco_Mac;
            nome = perfilLogado.nome;
            pontuacao_Total = perfilLogado.pontuacao_Total;
        }
    }

}
