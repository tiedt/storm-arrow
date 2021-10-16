using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("cidades")]
    public class CidadesController : Controller{

        protected readonly ICidadeRepository cidadeRepository;

        public CidadesController(ICidadeRepository cidadeRepository){
            this.cidadeRepository = cidadeRepository;
        }

        // GET cidades?id_estado={id_estado}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? id_estado){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await cidadeRepository.ListAll(id_estado);

                if (result.Data != null
                && ((List<Cidade>) result.Data).Count > 0){
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
