using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("estado")]
    public class EstadoController : Controller{

        protected readonly IEstadoRepository estadoRepository;

        public EstadoController(IEstadoRepository estadoRepository){
            this.estadoRepository = estadoRepository;
        }

        // GET estado/{id}
        [HttpGet("{id}", Name = "getEstadoID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id > 0)
                    result.Data = await estadoRepository.GetById(id);

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
