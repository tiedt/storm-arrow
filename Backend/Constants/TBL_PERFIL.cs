using SIMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Constants {
    
    public static class TBL_PERFIL {

        public const string NAME             = "PERFIL";
        public static FieldConst ID          = new FieldConst("SEQ_PERFILID", "ID");
        public static string ENDERECO_MAC    = "ENDERECO_MAC";
        public static string NOME            = "NOME";
        public static string PONTUACAO_TOTAL = "PONTUACAO_TOTAL";

    }
}
