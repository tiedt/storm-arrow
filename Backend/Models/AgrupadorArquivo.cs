namespace SIMP.Models{

    public class AgrupadorArquivo{

        public AgrupadorArquivo(){
            this.Arquivo = new Arquivo();
        }

        public int Nr_id { get; set; }

        public int Nr_agrupador { get; set; }

        public int Nr_id_arquivo { get; set; }

        public Arquivo Arquivo { get; set; }

    }

}
