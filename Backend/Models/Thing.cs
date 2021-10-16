using System;

namespace SIMP.Models{
    
    public class Thing{

        public int Id { get; set; }
        public string Nome { get; set; }
        public float Temperatura { get; set; }
        public float Umidade { get; set; }
        public float Luz_Dia { get; set; }
        public float Mili_Chuva { get; set; }
        public float Barometro { get; set; }
        public float Pressao { get; set; }
        public float Velocidade_Vento { get; set; }
        public float Direcao_Vento { get; set; }
        public DateTime Data { get; set; }
    }
}
