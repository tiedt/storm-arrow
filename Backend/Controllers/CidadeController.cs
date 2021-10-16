using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("cidade")]
    public class CidadeController : Controller{

        protected readonly ICidadeRepository cidadeRepository;

        public CidadeController(ICidadeRepository cidadeRepository){
            this.cidadeRepository = cidadeRepository;
        }

        // GET cidade/{id}
        [HttpGet("{id}", Name = "getCidadeID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id > 0)
                    result.Data = await cidadeRepository.GetById(id);

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
