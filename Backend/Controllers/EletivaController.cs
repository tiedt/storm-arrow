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

        // GET eletiva?valor={valor}&receive_at={receive_at}
        public async Task<IActionResult> Get(float valor, DateTime receive_at){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await eletivaRepository.Insert(new Eletiva() { Potenciometro = valor, Receive_at = receive_at });

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "404"; // Não encontrado
                    return NotFound(result);
                }
            }catch (Exception e){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }
    }
}
