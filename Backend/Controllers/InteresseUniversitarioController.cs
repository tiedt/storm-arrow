using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("universitario/interesse")]
    public class InteresseUniversitarioController : Controller{

        protected readonly IInteresseRepository interesseRepository;

        public InteresseUniversitarioController(IInteresseRepository interesseRepository){
            this.interesseRepository = interesseRepository;
        }

        // GET universitario/interesse/{id_universitario}
        [HttpGet("{id_universitario}", Name = "getInteresseUniversitario")]
        public async Task<IActionResult> Get(int id_universitario){

            ReturnRequest result = new ReturnRequest();
            try{
                if (id_universitario > 0)
                    result.Data = await interesseRepository.ListAllByUniversity(id_universitario);

                if (result.Data != null
                && ((List<Proposta>) result.Data).Count > 0){
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
