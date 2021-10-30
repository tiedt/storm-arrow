
[System.Serializable]
public class PerfilConfiguracoes{

    public int id;
    public int idPerfil;
    public string config;
    public string valor;

    public PerfilConfiguracoes() { }

    public override bool Equals(object obj) {
        if(obj == null)
            return false;
        if(obj == this)
            return true;
        if(obj.GetType() != this.GetType())
            return false;
        PerfilConfiguracoes other = (PerfilConfiguracoes) obj;
        return other.idPerfil == this.idPerfil
            && other.config.Equals(this.config, System.StringComparison.OrdinalIgnoreCase);
    }

}
