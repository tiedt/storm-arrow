using System;
using System.Collections.Generic;
using UnityEngine;

public class PerfilLogado{
    
    private Perfil perfil = null;
    private List<PerfilConfiguracoes> perfilConfiguracoes;
    private List<PontuacaoMusica> pontuacaoMusicas;

    public static PerfilLogado Instance = new PerfilLogado();
    public bool conectado { get => perfil != null; }
    public int id { get => perfil.id; }
    public string endereco_Mac { get => perfil.endereco_Mac; }
    public string nome { get => perfil.nome; }
    public int pontuacao_Total { get => perfil.pontuacao_Total; set => perfil.pontuacao_Total = value; }
    public List<PerfilConfiguracoes> Configuracoes { get => perfilConfiguracoes; }
    public List<PontuacaoMusica> PontuacaoMusicas { get => pontuacaoMusicas; }

    public int nivel { get => RetornaNivel();  }
    
    private int RetornaNivel() {
        if(pontuacao_Total > 0) { 
            int percentualAcrescimo = (pontuacao_Total / 700000) * 10000;
            return pontuacao_Total / (700000 + percentualAcrescimo);
        }else
            return 0;
    }

    public void ConectarPerfil(string nome) {
        try { 
            string enderecoMac = ServicosUtils.RetornaMelhorEnderecoMac();
            string enderecoipv4Perfil = $@"{Enderecos.Perfis}?macAddress={enderecoMac}&nome={nome}";            
            perfil = ServicosHttp<Perfil>.RetornaObjetoServidor(enderecoipv4Perfil).Result;
            if(perfil != null) {
                // Tabela perfil configurações
                string enderecoipv4PerfilConfiguracoes = $@"{Enderecos.PerfilConfiguracoes}?IdPerfil={perfil.id}";
                perfilConfiguracoes = ServicosHttp<List<PerfilConfiguracoes>>.RetornaObjetoServidor(enderecoipv4PerfilConfiguracoes).Result;
                if(perfilConfiguracoes != null) {
                    foreach (PerfilConfiguracoes configuracao in perfilConfiguracoes) {
                        if(configuracao.config.Equals("VolumePrincipal", StringComparison.OrdinalIgnoreCase))
                            Musica.DefinirVolumeMusica(null, float.Parse(configuracao.valor));
                    }
                }
                // Tabela pontuação músicas
                string enderecoipv4PontuacoesMusicas = $@"{Enderecos.PontuacaoMusicas}?IdPerfil={perfil.id}";
                pontuacaoMusicas = ServicosHttp<List<PontuacaoMusica>>.RetornaObjetoServidor(enderecoipv4PontuacoesMusicas).Result;
                if(pontuacaoMusicas is null) {
                    pontuacaoMusicas = new List<PontuacaoMusica>();
                }
            }
        } catch (Exception) {
            perfil = null;
            perfilConfiguracoes = null;
            pontuacaoMusicas = null;
        }
    }

    public void DesconectarPerfil() {
        perfil = null;
        perfilConfiguracoes = null;
        pontuacaoMusicas = null;
    }

    public void AtualizaConfiguracaoPerfil(PerfilConfiguracoes configuracao) {
        for(int i = 0; i < perfilConfiguracoes.Count; i++) {
            if((perfilConfiguracoes[i].idPerfil == configuracao.idPerfil)
            && (perfilConfiguracoes[i].config.Equals(configuracao.config, StringComparison.OrdinalIgnoreCase))) {
                perfilConfiguracoes[i].valor = configuracao.valor;
            }
        }
        
    }

}
