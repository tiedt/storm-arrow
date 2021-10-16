using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("cursos")]
    public class CursosController : Controller{

        protected readonly ICursoRepository cursoRepository;
        protected readonly IArquivoRepository arquivoRepository;

        public CursosController(ICursoRepository cursoRepository, IArquivoRepository arquivoRepository){
            this.cursoRepository = cursoRepository;
            this.arquivoRepository = arquivoRepository;
        }

        // GET cursos
        [HttpGet]
        public async Task<IActionResult> Get(){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await cursoRepository.ListAll();

                if (result.Data != null
                && ((List<Curso>) result.Data).Count > 0){
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
