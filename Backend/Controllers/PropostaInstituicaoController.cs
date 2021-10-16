using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("propostas/instituicao")]
    public class PropostaInstituicaoController : Controller{

        protected readonly IPropostaRepository propostaRepository;

        public PropostaInstituicaoController(IPropostaRepository propostaRepository){
            this.propostaRepository = propostaRepository;
        }

        // GET propostas/instituicao/{id_instituicao}
        [HttpGet("{id_instituicao}", Name = "getPropostaInstituicaoID")]
        public async Task<IActionResult> Get(int id_instituicao){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id_instituicao > 0)
                    result.Data = await propostaRepository.GetByInstitution(id_instituicao);

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
