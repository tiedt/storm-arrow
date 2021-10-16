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

        // GET PontuacaoMusicas?idPerfil={idPerfil}&estilo={estilo}&musica{musica}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int idPerfil, [FromQuery] string estilo, [FromQuery] string musica){
            
            ReturnRequest result = new ReturnRequest();
            
            try{
                if(idPerfil > 0
                && !String.IsNullOrEmpty(estilo)
                && !String.IsNullOrEmpty(musica))
                    result.Data = await pontuacaoMusicaRepository.ListAllByProfileStyleMusic(idPerfil, estilo, musica);

                if (result.Data != null
                && ((List<PontuacaoMusica>) result.Data).Count > 0){
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
