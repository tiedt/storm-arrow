using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Models {
    
    public class PerfilConfiguracoes {

        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public string Config { get; set; }
        public string Valor { get; set; }

    }

}
