namespace SIMP.Models{
    
    public class Universitario : Usuario{

        public Universitario(){ }

        public Universitario(Usuario usuario){
            this.Ds_email             = usuario.Ds_email;
            this.Ds_nome_exibido      = usuario.Ds_nome_exibido;
            this.Ds_senha             = usuario.Ds_senha;
            this.Nr_id_usuario        = usuario.Nr_id;
            this.Nr_agrupador_arquivo = usuario.Nr_agrupador_arquivo;
        }

        public int Nr_id_usuario { get; set; }

        public int Nr_id_instituicao { get; set; }

        public int Nr_id_cidade { get; set; }

        public int Nr_id_estado { get; set; }

        public string Ds_nome { get; set; }
        
        public string Ds_sobrenome { get; set; }
        
        public string Ds_telefone { get; set; }
        
        public string Ds_grau { get; set; }

    }
}
