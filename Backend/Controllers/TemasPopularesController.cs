using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("temaspopulares")]
    public class TemasPopularesController : Controller{

        protected readonly IPropostaRepository propostaRepository;

        public TemasPopularesController(IPropostaRepository propostaRepository){
            this.propostaRepository = propostaRepository;
        }

        // GET temaspopulares
        [HttpGet]
        public async Task<IActionResult> Get(){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await propostaRepository.GetMostAccessed();

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
