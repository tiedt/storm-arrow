using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("proposta")]
    public class PropostaController : Controller{

        protected readonly IPropostaRepository propostaRepository;

        public PropostaController(IPropostaRepository propostaRepository){
            this.propostaRepository = propostaRepository;
        }

        // GET proposta?id={id}&id_usuario={id_usuario}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id, int? id_usuario){

            ReturnRequest result = new ReturnRequest();

            try{
                if (id > 0)
                    result.Data = await propostaRepository.GetById(id, id_usuario);

                if (result.Data != null){
                    result.Status = "200"; // OK
                    return Accepted(result);
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

        // POST proposta
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Proposta proposta, [FromForm(Name ="fileInput")] List<IFormFile> files){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await propostaRepository.Insert(proposta, files);
                if (result.Data == null){
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

        // PUT proposta
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Proposta proposta, [FromForm(Name ="fileInput")] List<IFormFile> files){

            ReturnRequest result = new ReturnRequest();

            try{
                if (await propostaRepository.GetById(proposta.Nr_id, 0) != null)
                    result.Data = await propostaRepository.Update(proposta, files);
                else{
                    result.Status = "404";
                    return NotFound(result);
                }

                if (result.Data == null){
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
