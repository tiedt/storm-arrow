using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("universitarios/proposta")]
    public class UniversitariosPropostaController : Controller{

        protected readonly IPropostaUniversitarioRepository propostaUniversitarioRepository;

        public UniversitariosPropostaController(IPropostaUniversitarioRepository propostaUniversitarioRepository){
            this.propostaUniversitarioRepository = propostaUniversitarioRepository;
        }

        // GET universitarios/proposta/{id_proposta}
        [HttpGet("{id_proposta}", Name = "getPropostaUnviersitarioIDProposta")]
        public async Task<IActionResult> Get(int id_proposta){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id_proposta > 0)
                    result.Data = await propostaUniversitarioRepository.ListAllByProposals(id_proposta);

                if (result.Data != null
                && ((List<Universitario>) result.Data).Count > 0){
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
