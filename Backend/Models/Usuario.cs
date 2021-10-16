using System.Collections.Generic;

namespace SIMP.Models{

    public class Usuario{
        
        public int Nr_id { get; set; }        
        public string Ds_email { get; set; }        
        public string Ds_senha { get; set; }        
        public string Ds_nome_exibido { get; set; }
        public int Nr_agrupador_arquivo { get; set; }
        public IEnumerable<AgrupadorArquivo> AgrupadorArquivo { get; set; }

    }
}
