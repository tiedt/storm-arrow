using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("universitarios")]
    public class UniversitariosController : Controller{

        protected readonly IUniversitarioRepository universitarioRepository;

        public UniversitariosController(IUniversitarioRepository universitarioRepository){
            this.universitarioRepository = universitarioRepository;
        }

        // GET universitarios
        [HttpGet]
        public async Task<IActionResult> Get(){

            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await universitarioRepository.ListAll();

                if (result.Data != null
                 && ((List<Universitario>) result.Data).Count > 0) { 
                    result.Status = "200"; // OK
                    return Ok(result);
                }
                else {
                    result.Status = "404"; // Não encontrado
                    result.Data = null;
                    return NotFound(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }
    }
}
