using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class PerfilLogado : MonoBehaviour
{
    
    public static Perfil perfil;

    public static void ConectarPerfil() {
        perfil = null;
        string macAddress = "74-D0-2B-9E-1F-63";
        string nome = "xXxcollazzoxXx";
        string enderecoipv4 = $@"{Enderecos.Perfis}?macAddress={macAddress}&nome={nome}";
        perfil = Servicos<Perfil>.RetornaObjetoServidor(enderecoipv4).Result;
    }

    public static void DesconectarPerfil() {
        perfil = null;
    }

}
