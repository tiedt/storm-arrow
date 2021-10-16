namespace SIMP.Models{

    public class PropostaUniversitario{

        public PropostaUniversitario() { }
        public PropostaUniversitario(int Nr_id, int Nr_id_universitario, int Nr_id_proposta){
            this.Nr_id = Nr_id;
            this.Nr_id_universitario = Nr_id_universitario;
            this.Nr_id_proposta = Nr_id_proposta;
        }
        public int Nr_id { get; set; }
        
        public int Nr_id_universitario { get; set; }

        public int Nr_id_proposta { get; set; }

        
    }
}
