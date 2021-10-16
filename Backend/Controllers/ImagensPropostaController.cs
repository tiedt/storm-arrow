using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("imagens/proposta")]
    public class ImagensPropostaController : Controller{

        protected readonly IPropostaRepository propostaRepository;
        
        public ImagensPropostaController(IPropostaRepository propostaRepository){
            this.propostaRepository = propostaRepository;
        }

        // GET imagens/proposta/{id}
        [HttpGet("{id}", Name = "getImagensPropostaID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();
            try{                
                if (id > 0)
                    result.Data = await propostaRepository.GetImages(id);

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "404"; // Não encontrado
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
