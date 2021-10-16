using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Models {
    
    public class PontuacaoMusica {

        public int Id { get; set; }

        public int IdPerfil { get; set; }

        public string Estilo { get; set; }

        public string Musica { get; set; }

        public int Pontuacao { get; set; }


    }

}
