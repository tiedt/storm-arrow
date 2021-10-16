using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers {
    
    [ApiController]
    [Route("PontuacaoMusica")]
    public class PontuacaoMusicaController : Controller{

        protected readonly IPontuacaoMusicaRepository pontuacaoMusicaRepository;
        public PontuacaoMusicaController(IPontuacaoMusicaRepository pontuacaoMusicaRepository){
            this.pontuacaoMusicaRepository = pontuacaoMusicaRepository;
        }

        // POST PontuacaoMusica
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PontuacaoMusica model){
            ReturnRequest result = new ReturnRequest();
            try{                
                result.Data = await pontuacaoMusicaRepository.Insert(model);
                if (result.Data != null){
                    result.Status = "200";
                    return Ok(result);
                }else{
                    result.Status = "409";
                    return Conflict(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // PUT PontuacaoMusica
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PontuacaoMusica model){
            
            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await pontuacaoMusicaRepository.Update(model);
                
                if (result.Data != null){
                    result.Status = "200";
                    return Ok(result);
                }else{
                    result.Status = "404";
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
