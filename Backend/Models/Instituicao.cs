﻿namespace SIMP.Models{

    public class Instituicao : Usuario{

        public Instituicao() { }

        public Instituicao(Usuario usuario){
            this.Ds_email             = usuario.Ds_email;
            this.Ds_nome_exibido      = usuario.Ds_nome_exibido;
            this.Ds_senha             = usuario.Ds_senha;
            this.Nr_id_usuario        = usuario.Nr_id;
            this.Nr_agrupador_arquivo = usuario.Nr_agrupador_arquivo;
        }
        
        public int Nr_id_cidade { get; set; }
        
        public int Nr_id_estado { get; set; }
        
        public int Nr_id_usuario { get; set; }

        public string Ds_razao_social { get; set; }

        public string Ds_ramo { get; set; }

        public string Cd_cnpj { get; set; }

        public string Ds_resumo { get; set; }
        
        public string Ds_descricao { get; set; }

        public string Ds_email_contato { get; set; }

        public string Ds_telefone { get; set; }

        public string Ds_horario_funcionamento { get; set; }

    }
}
