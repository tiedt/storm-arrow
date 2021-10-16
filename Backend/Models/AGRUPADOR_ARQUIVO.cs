namespace SIMP.Models{

    public class AgrupadorArquivo{

        public int nr_id { get; set; }

        public int nr_agrupador { get; set; }

        public int nr_id_arquivo { get; set; }

        public Arquivo arquivo { get; set; }

    }

}
