using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Controllers {

    [ApiController]
    [Route("perfis")]
    public class PerfisController : Controller{

        protected readonly IPerfilRepository perfilRepository;
        
        public PerfisController(IPerfilRepository perfilRepository){
            this.perfilRepository = perfilRepository;
        }

        // GET perfis?MacAddress={MacAddress}&nome={nome}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string MacAddress, [FromQuery] string? nome){
            
            ReturnRequest result = new ReturnRequest();
            
            try{
                if(!String.IsNullOrEmpty(MacAddress))
                    if(String.IsNullOrEmpty(nome)) { 
                        result.Data = await perfilRepository.ListAllMacAddress(MacAddress);
                        if (result.Data != null
                        && ((List<Perfil>) result.Data).Count > 0){
                            result.Status = "200"; // OK
                            return Ok(result);
                        }
                    } else { 
                        result.Data = await perfilRepository.GetByMacAddrressName(MacAddress, nome);
                        if (result.Data != null){
                            result.Status = "200"; // OK
                            return Ok(result);
                        }
                    }       
                result.Status = "404"; // Não encontrado
                result.Data = null;
                return NotFound(result);
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }

        }
    }
}
