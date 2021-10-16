using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("curso")]
    public class CursoController : Controller{

        protected readonly ICursoRepository cursoRepository;

        public CursoController(ICursoRepository cursoRepository){
            this.cursoRepository = cursoRepository;
        }

        // GET curso/{id}
        [HttpGet("{id}", Name = "getCursoID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id > 0)
                    result.Data = await cursoRepository.GetById(id);

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
