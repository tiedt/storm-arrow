using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("token")]
    public class TokenController: Controller{

        private readonly ITokenRepository tokenRepository;
        public TokenController(ITokenRepository tokenRepository){
            this.tokenRepository = tokenRepository;
        }

        // GET token?Token={Token}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string Token){

            ReturnRequest result = new ReturnRequest();
                        
            try{
                if (!String.IsNullOrEmpty(Token)){
                    Token model = await tokenRepository.GetByToken(Token);
                    result.Data = model.Dt_data_limite > DateTime.Now;
                }
                if (result.Data != null
                && ((bool) result.Data)){
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

        // POST token
        [HttpPost]
        public async Task<IActionResult> Post(){

            ReturnRequest result = new ReturnRequest();
                        
            try{
                result.Data = await tokenRepository.Insert();
                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "409";
                    result.Data = null;
                    return Conflict(result);
                }
            }catch (Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }
    }
}
