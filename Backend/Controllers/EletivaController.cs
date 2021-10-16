using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Controllers {
    
    [ApiController]
    [Route("eletiva")]
    public class EletivaController : Controller{

        protected readonly IEletivaRepository eletivaRepository;

        public EletivaController(IEletivaRepository eletivaRepository){
            this.eletivaRepository = eletivaRepository;
        }

        // GET eletiva?valor={valor}
        public async Task<IActionResult> Get(float valor){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await eletivaRepository.Insert(new Eletiva() { Potenciometro = valor });

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "404"; // Não encontrado
                    return NotFound(result);
                }
            }catch (Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }
    }
}
