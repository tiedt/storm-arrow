using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("weatherestation")]
    public class ThingIOTController : Controller{

        protected readonly IThingRepository thingRepository;

        public ThingIOTController(IThingRepository thingRepository){
            this.thingRepository = thingRepository;
        }

        // GET weatherestation?nome={nome}&temperatura={temp}&umidade={umid}&LuzDia={LuzDia}&MiliChuva={MiliChuva}&Barometro={Barometro}&Pressao={Pressao}&VelocidadeVento={VelocidadeVento}&DirecaoVento={DirecaoVento}
        [HttpGet]
        public async Task<int> Get([FromQuery] string Nome, [FromQuery] float Temperatura, [FromQuery] float Umidade, [FromQuery] float LuzDia, [FromQuery] float MiliChuva, [FromQuery] float Barometro, [FromQuery] float Pressao, [FromQuery] float VelocidadeVento, [FromQuery] float DirecaoVento ){
            try{
                
                Thing Model = new Thing(){
                    Nome = Nome,
                    Temperatura = Temperatura,
                    Umidade = Umidade,
                    Luz_Dia = LuzDia,
                    Mili_Chuva = MiliChuva,
                    Barometro = Barometro,
                    Pressao = Pressao,
                    Velocidade_Vento = VelocidadeVento,
                    Direcao_Vento = DirecaoVento,
                    Data = DateTime.Now
                };
                await thingRepository.Insert(Model);
                return await thingRepository.ListAll(Nome);
            }catch (Exception){
                return 0;
            }
        }
    }
}
