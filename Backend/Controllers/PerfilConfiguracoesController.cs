using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers {
    
    [ApiController]
    [Route("PerfilConfiguracoes")]
    public class PerfilConfiguracoesController : Controller{

        protected readonly IPerfilConfiguracoesRepository perfilConfiguracoesRepository;

        public PerfilConfiguracoesController(IPerfilConfiguracoesRepository perfilConfiguracoesRepository){
            this.perfilConfiguracoesRepository = perfilConfiguracoesRepository;
        }

        // GET PerfilConfiguracoes
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int IdPerfil){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await perfilConfiguracoesRepository.ListByPerfilConfig(IdPerfil);

                if (result.Data != null
                && ((List<PerfilConfiguracoes>) result.Data).Count > 0){
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

        // POST PerfilConfiguracoes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PerfilConfiguracoes model){
            ReturnRequest result = new ReturnRequest();
            try{                
                result.Data = await perfilConfiguracoesRepository.Insert(model);
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

        // PUT PerfilConfiguracoes
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PerfilConfiguracoes model){
            
            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await perfilConfiguracoesRepository.Update(model);
                
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
