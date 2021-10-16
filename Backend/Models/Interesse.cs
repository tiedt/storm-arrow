using System;

namespace SIMP.Models{
    
    public class Interesse{
        public int Nr_id { get; set; }
        public int Nr_id_proposta { get; set; }
        public int Nr_id_universitario { get; set; }
        public DateTime Dt_geracao { get; set; }
        public string? Ds_status { get; set; }
    }

}
