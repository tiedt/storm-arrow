using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("imagens/usuario")]
    public class ImagensUsuarioController : Controller{

        protected readonly IUsuarioRepository usuarioRepository;
        
        public ImagensUsuarioController(IUsuarioRepository usuarioRepository){
            this.usuarioRepository = usuarioRepository;
        }

        // GET imagens/usuario/{id}
        [HttpGet("{id}", Name = "getImagensUsuarioID")]
        public async Task<IActionResult> Get(int id){

            ReturnRequest result = new ReturnRequest();
            try{                
                if (id > 0)
                    result.Data = await usuarioRepository.GetImages(id);

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Ok(result);
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
    }
}
