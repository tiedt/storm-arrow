namespace SIMP.Models{

    public class LogAcesso{

        public LogAcesso() { }

        public LogAcesso(int Nr_id_proposta, int Nr_id_usuario){
            this.Nr_id_proposta = Nr_id_proposta;
            this.Nr_id_usuario = Nr_id_usuario;
        }
        public int Nr_id { get; set; }

        public int Nr_id_proposta { get; set; }

        public int Nr_id_usuario { get; set; }

    }
}
