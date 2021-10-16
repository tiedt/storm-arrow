using System;
using System.Collections.Generic;

namespace SIMP.Models{
    
    public class Proposta{
        
        public int Nr_id { get; set; }

        public int Nr_id_curso { get; set; }

        public Curso Curso { get; set; }

        public int Nr_id_instituicao { get; set; }

        public Instituicao Instituicao { get; set; }

        public string Ds_nome { get; set; }

        public string Ds_desc_projeto { get; set; }

        public string Cd_status { get; set; }

        public string Ds_requisito { get; set; }

        public int Qt_participantes { get; set; }

        public int Nr_duracao { get; set; }

        public string Ds_info_contatos { get; set; }

        public string Ds_tipo { get; set; }

        public DateTime? Dt_geracao { get; set; } 

        public int Nr_agrupador_arquivo { get; set; }
        
        public IEnumerable<AgrupadorArquivo> AgrupadorArquivo { get; set; }

        public IEnumerable<Universitario> Universitarios { get; set; }

        public bool Usuario_interessado { get; set; }
    }
}

