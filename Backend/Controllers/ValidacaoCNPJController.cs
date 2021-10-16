using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("validacao/cnpj")]
    public class ValidacaoCNPJController: Controller{

        // GET validacao/cnpj?cnpj={cnpj}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string cnpj){

            ReturnRequest result = new ReturnRequest();
                        
            try{                
                if (await Task.Run(() => ValidatingClass.CNPJ(cnpj))){
                    result.Status = "200"; // OK
                    result.Data = true;
                    return Ok(result);
                }else{
                    result.Status = "404"; // Não encontrado
                    result.Data = false;
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
