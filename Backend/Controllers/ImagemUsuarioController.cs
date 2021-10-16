using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("imagem/usuario")]
    public class ImagemUsuarioController : Controller{

        protected readonly IUsuarioRepository usuarioRepository;
        
        public ImagemUsuarioController(IUsuarioRepository usuarioRepository){
            this.usuarioRepository = usuarioRepository;
        }

        // POST imagem/usuario/{id}?nr_agrupador={nr_agrupador}
        [HttpPost("{id}", Name = "postImagemUsuarioID")]
        public async Task<IActionResult> Post(int id, [FromQuery] int nr_agrupador, [FromForm(Name ="fileInput")] List<IFormFile> files){

            ReturnRequest result = new ReturnRequest();
            try{                
                if (id > 0
                && nr_agrupador <= 0)
                    result.Data = await usuarioRepository.InsertImage(id, nr_agrupador, files);

                if (result.Data != null
                && ((bool) result.Data)){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "409";
                    result.Data = null;
                    return Conflict(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // PUT imagem/usuario?id_arquivo={id_arquivo}
        [HttpPut]
        public async Task<IActionResult> Put([FromForm(Name ="fileInput")] IFormFile file, [FromQuery] int id_arquivo){

            ReturnRequest result = new ReturnRequest();
            try{                
                if (id_arquivo > 0)
                    result.Data = await usuarioRepository.UpdateImage(id_arquivo, file);

                if (result.Data != null
                && ((bool) result.Data)){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "409";
                    result.Data = null;
                    return Conflict(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }

        // DELETE imagem/usuario/{id}?id_arquivo={id_arquivo}
        [HttpDelete("{id}", Name = "deleteImagemUsuarioID")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int id_arquivo){

            ReturnRequest result = new ReturnRequest();
            try{                
                if (id_arquivo > 0)
                    result.Data = await usuarioRepository.DeleteImage(id, id_arquivo);

                if (result.Data != null
                && ((bool) result.Data)){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "409";
                    result.Data = null;
                    return Conflict(result);
                }
            }catch(Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }
    }
}
