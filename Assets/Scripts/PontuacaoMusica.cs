

[System.Serializable]
public class PontuacaoMusica{

    public int id;
    public int idPerfil;
    public string estilo;
    public string musica;
    public int pontuacao;

    public override bool Equals(object obj) {
        if(obj == null)
            return false;
        if(obj == this)
            return true;
        if(obj.GetType() != this.GetType())
            return false;
        PontuacaoMusica other = (PontuacaoMusica) obj;
        return other.idPerfil == this.idPerfil
            && other.estilo.Equals(this.estilo)
            && other.musica.Equals(this.musica);
    }

}
