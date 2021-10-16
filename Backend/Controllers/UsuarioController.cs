using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : Controller{

        protected readonly IUsuarioRepository usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository){
            this.usuarioRepository = usuarioRepository;
        }

        // GET usuario/{id}
        [HttpGet("{id}", Name = "getUsuarioID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();
            try{                
                if (id > 0)
                    result.Data = await usuarioRepository.GetById(id);

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Accepted(result);
                }else{
                    result.Status = "404"; // Não encontrado
                    return NotFound(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // GET usuario?email={email}&senha={senha}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string email, string senha){

            ReturnRequest result = new ReturnRequest();
            try{                
                if (!String.IsNullOrEmpty(email)
                && !String.IsNullOrEmpty(senha))
                    result.Data = await usuarioRepository.GetByEmailPassword(email, senha);

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Accepted(result);
                }else{
                    result.Status = "404"; // Não encontrado
                    return NotFound(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // POST usuario
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] System.Text.Json.JsonElement obj){
            ReturnRequest result = new ReturnRequest();
            try{                
                result.Data = await usuarioRepository.Insert(obj);
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

        // PUT usuario
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] System.Text.Json.JsonElement obj){
            
            ReturnRequest result = new ReturnRequest();
            try{
                result.Data = await usuarioRepository.Update(obj);
                
                if (result.Data != null){
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
