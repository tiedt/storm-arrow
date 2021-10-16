using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("instituicoes")]
    public class InstituicoesController : Controller{

        protected readonly IInstituicaoRepository instituicaoRepository;

        public InstituicoesController(IInstituicaoRepository instituicaoRepository){
            this.instituicaoRepository = instituicaoRepository;
        }

        // GET instituicoes?ds_ramo={dsramo}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? ds_ramo){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await instituicaoRepository.ListAll(ds_ramo);

                if (result.Data != null
                && ((List<Instituicao>) result.Data).Count > 0){
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
