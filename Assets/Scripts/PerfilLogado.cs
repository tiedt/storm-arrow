using System;

public class PerfilLogado{
    
    private Perfil perfil = null;
    public static PerfilLogado Instance = new PerfilLogado();
    public bool conectado { get => perfil != null; }
    public int id { get => perfil.id; }
    public string endereco_Mac { get => perfil.endereco_Mac; }
    public string nome { get => perfil.nome; }
    public int pontuacao_Total { get => perfil.pontuacao_Total; set => perfil.pontuacao_Total = value; }

    public void ConectarPerfil(string nome) {
        try { 
            string enderecoMac = ServicosUtils.RetornaMelhorEnderecoMac();
            string enderecoipv4Perfil = $@"{Enderecos.Perfis}?macAddress={enderecoMac}&nome={nome}";
            perfil = ServicosHttp<Perfil>.RetornaObjetoServidor(enderecoipv4Perfil).Result;
        } catch (Exception) {
            perfil = null;
        }
    }

    public void DesconectarPerfil() {
        perfil = null;
    }

}
