using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Controllers{
 
    [ApiController]
    [Route("validacao/cpf")]
    public class ValidacaoCPFController: Controller{

        // GET validacao/cpf?cpf={cpf}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string cpf){

            ReturnRequest result = new ReturnRequest();

            try{
                if (await Task.Run(() => ValidatingClass.CPF(cpf))){
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
