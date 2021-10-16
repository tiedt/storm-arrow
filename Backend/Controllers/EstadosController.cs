using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("estados")]
    public class EstadosController : Controller{

        protected readonly IEstadoRepository estadoRepository;

        public EstadosController(IEstadoRepository estadoRepository){
            this.estadoRepository = estadoRepository;
        }

        // GET estados?id_pais={id_pais}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? id_pais){

            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await estadoRepository.ListAll(id_pais);
                if (result.Data != null
                && ((List<Estado>) result.Data).Count > 0){
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
