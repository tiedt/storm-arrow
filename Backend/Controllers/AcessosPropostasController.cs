using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("acessospropostas")]
    public class AcessosPropostasController : Controller{

        protected readonly IAcessosPropostaRepository acessosPropostaRepository;

        public AcessosPropostasController(IAcessosPropostaRepository acessosPropostaRepository){
            this.acessosPropostaRepository = acessosPropostaRepository;
        }

        // GET acessospropostas
        [HttpGet]
        public async Task<IActionResult> Get(){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await acessosPropostaRepository.ListAll();

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
