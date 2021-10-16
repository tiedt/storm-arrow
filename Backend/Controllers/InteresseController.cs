using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("interesse")]
    public class InteresseController : Controller{

        private readonly IInteresseRepository interesseRepository;
        public InteresseController(IInteresseRepository interesseRepository){
            this.interesseRepository = interesseRepository;
        }
        
        // POST interesse
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Interesse model){
            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await interesseRepository.Insert(model);
                if (result.Data == null
                && !((bool) result.Data)){
                    result.Status = "409";
                    return Conflict(result);
                }else{
                    result.Status = "200";
                    return Ok(result);
                }
            }catch (Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // DELETE interesse?id_proposta={id_proposta}&id_universitario={id_universitario}
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id_proposta, [FromQuery] int id_universitario){
            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await interesseRepository.Delete(new Interesse(){Nr_id_proposta = id_proposta, Nr_id_universitario = id_universitario});
                if (result.Data == null
                && !((bool) result.Data)){
                    result.Status = "409";
                    return Conflict(result);
                }else{
                    result.Status = "200";
                    return Ok(result);
                }
            }catch (Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // PUT interesse?id_proposta={id_proposta}&id_universitario={id_universitario}
        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] int id_proposta, [FromQuery] int id_universitario){
            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await interesseRepository.AcceptInterest(new Interesse(){ Nr_id_proposta = id_proposta, Nr_id_universitario = id_universitario });
                if (result.Data == null
                || !((bool) result.Data)){
                    result.Status = "409";
                    result.Status = null;
                    return Conflict(result);
                }else{
                    result.Status = "200";
                    return Ok(result);
                }
            }catch (Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }
    }
}
