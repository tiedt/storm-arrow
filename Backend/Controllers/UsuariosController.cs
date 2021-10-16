using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : Controller{

        protected readonly IUsuarioRepository usuarioRepository;
        
        public UsuariosController(IUsuarioRepository usuarioRepository){
            this.usuarioRepository = usuarioRepository;
        }

        // GET usuarios
        [HttpGet]
        public async Task<IActionResult> Get(){
            
            ReturnRequest result = new ReturnRequest();
            
            try{
                result.Data = await usuarioRepository.ListAll();

                if (result.Data != null
                && ((List<Object>) result.Data).Count > 0){
                    result.Status = "200"; // OK
                    return Ok(result);
                }
                else{
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
