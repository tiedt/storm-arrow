using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("propostas/universitario")]
    public class PropostaUniversitarioController : Controller{

        protected readonly IPropostaUniversitarioRepository propostaUniversitarioRepository;

        public PropostaUniversitarioController(IPropostaUniversitarioRepository propostaUniversitarioRepository){
            this.propostaUniversitarioRepository = propostaUniversitarioRepository;
        }

        // GET propostas/universitario/{id_universitario}?status={status}
        [HttpGet("{id_universitario}", Name = "getPropostaUnviersitarioIDUniversitarioStatus")]
        public async Task<IActionResult> Get(int id_universitario, [FromQuery] string? status){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id_universitario > 0)
                    result.Data = await propostaUniversitarioRepository.ListAllByUniversity(id_universitario, status);

                if (result.Data != null
                && ((List<Proposta>) result.Data).Count > 0){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "404"; // Não encontrado
                    result.Data = null;
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
