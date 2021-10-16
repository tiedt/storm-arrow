using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("universitario")]
    public class UniversitarioController : Controller{

        protected readonly IUniversitarioRepository universitarioRepository;

        public UniversitarioController(IUniversitarioRepository universitarioRepository){
            this.universitarioRepository = universitarioRepository;
        }

        // GET universitario/{id}
        [HttpGet("{id}", Name = "getUniversitarioID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();
            
            try{
                if (id > 0)
                    result.Data = await universitarioRepository.GetById(id);

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Accepted(result);
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

        // POST universitario
        /*[HttpPost]
        public async Task<IActionResult> Post([FromBody] Universitario universitario){
            
            ReturnRequest result = new ReturnRequest();
            
            try{
                //if (universitarioRepository.getById(universitario.id) == null)
                    result.data = universitarioRepository.add(universitario).Result;
                //else{
                //    result.status = "401";
                //    return Unauthorized(result);
                //}
                if (result.data == null){
                    result.status = "409";
                    return Conflict(result);
                }else{
                    result.status = "200";
                    return Ok(result);
                }
            }catch (Exception e){
                result.status = "409";
                result.data = null;
                return Conflict(result);
            }
        }*/

        // PUT universitario
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Universitario universitario){

            ReturnRequest result = new ReturnRequest();
            
            try{
                if (await universitarioRepository.GetById(universitario.Nr_id) != null)
                    result.Data = await universitarioRepository.Update(universitario);
                else{
                    result.Status = "404";
                    return NotFound(result);
                }

                if (result.Data == null
                || (!(bool) result.Data)){
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
    }
}
