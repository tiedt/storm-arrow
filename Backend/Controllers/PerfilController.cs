using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers {

    [ApiController]
    [Route("Perfil")]
    public class PerfilController : Controller{

        protected readonly IPerfilRepository perfilRepository;
        public PerfilController(IPerfilRepository perfilRepository){
            this.perfilRepository = perfilRepository;
        }

        // GET perfil?nome={nome}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string nome) {
            ReturnRequest result = new ReturnRequest();
            try{                
                result.Data = await perfilRepository.GetByName(nome);
                if (result.Data != null){
                    result.Status = "200";
                    return Ok(result);
                }else{
                    result.Status = "409";
                    return Conflict(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // POST perfil
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Perfil model){
            ReturnRequest result = new ReturnRequest();
            try{                
                result.Data = await perfilRepository.Insert(model);
                if ((result.Data != null)
                && ((bool) result.Data)){
                    result.Status = "200";
                    return Ok(result);
                }else{
                    result.Status = "409";
                    return Conflict(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // PUT perfil
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Perfil model){
            
            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await perfilRepository.Update(model);
                
                if (result.Data != null
                && ((bool) result.Data)){
                    result.Status = "200";
                    return Ok(result);
                }else{
                    result.Status = "404";
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
