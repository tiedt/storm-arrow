using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("recomendacoes")]
    public class RecomendacoesController : Controller{
        
        protected readonly IPropostaRepository propostaRepository;

        public RecomendacoesController(IPropostaRepository propostaRepository){
            this.propostaRepository = propostaRepository;
        }

        // GET recomendacoes/{nr_id_usuario}
        [HttpGet("{nr_id_usuario}", Name = "getRecomendacoesNR_ID_USUARIO")]
        public async Task<IActionResult> Get(int nr_id_usuario) {

            ReturnRequest result = new ReturnRequest();

            try{
                if (nr_id_usuario > 0)
                    result.Data = await propostaRepository.GetRecommendations(nr_id_usuario);

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
