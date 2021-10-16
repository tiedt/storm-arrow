using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("pais")]
    public class PaisController : Controller{

        protected readonly IPaisRepository paisRepository;

        public PaisController(IPaisRepository paisRepository){
            this.paisRepository = paisRepository;
        }

        // GET pais/{id}
        [HttpGet("{id}", Name = "getPaisID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id > 0)
                    result.Data = await paisRepository.GetById(id);

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
