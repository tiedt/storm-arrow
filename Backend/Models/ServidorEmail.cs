namespace SIMP.Models{

    public class ServidorEmail{

        public int Nr_id { get; set; }
        public string Ds_nome { get; set; }
        public string Ds_endereco_smtp { get; set; }
        public int Nr_porta { get; set; }
        public int Nr_usa_ssl { get; set; }
    }
}
