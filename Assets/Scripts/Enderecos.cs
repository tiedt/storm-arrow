public static class Enderecos
{
    private readonly static string Base = "http://stormarrow.ddns.net:5000";
    /*
     * Get
     */
    public readonly static string Perfis = $@"{Base}/Perfis";
    /*
     * Put, Post
     */
    public readonly static string Perfil = $@"{Base}/Perfil";
    /*
     * Get
     */
    public readonly static string PontuacaoMusicas = $@"{Base}/PontuacaoMusicas";
    /* 
     * Put, Post
     */
    public readonly static string PontuacaoMusica = $@"{Base}/PontuacaoMusica";
}
