using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers {
    
    [ApiController]
    [Route("PontuacaoMusicas")]
    public class PontuacaoMusicasController : Controller{

        protected readonly IPontuacaoMusicaRepository pontuacaoMusicaRepository;
        
        public PontuacaoMusicasController(IPontuacaoMusicaRepository pontuacaoMusicaRepository){
            this.pontuacaoMusicaRepository = pontuacaoMusicaRepository;
        }

        // GET PontuacaoMusicas?idPerfil={idPerfil}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int idPerfil){
            
            ReturnRequest result = new ReturnRequest();
            
            try{
                if(idPerfil > 0)
                    result.Data = await pontuacaoMusicaRepository.ListAllByProfile(idPerfil);

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Ok(result);
                }
                else{
                    result.Status = "404"; // Não encontrado
                    result.Data = null;
                    return NotFound(result);
                }

            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }

        }
    }
}
