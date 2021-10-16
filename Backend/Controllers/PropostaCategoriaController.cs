using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("propostas/categoria")]
    public class PropostaCategoriaController : Controller{

        protected readonly IPropostaRepository propostaRepository;

        public PropostaCategoriaController(IPropostaRepository propostaRepository){
            this.propostaRepository = propostaRepository;
        }

        // GET propostas/categoria/{id}
        [HttpGet("{id_categoria}", Name = "getPropostaCategoriaID")]
        public async Task<IActionResult> Get(int id_categoria){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id_categoria > 0)
                    result.Data = await propostaRepository.GetByCategory(id_categoria);

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
