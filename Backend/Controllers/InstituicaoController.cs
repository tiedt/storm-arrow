using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("instituicao")]
    public class InstituicaoController : Controller{

        protected readonly IInstituicaoRepository instituicaoRepository;

        public InstituicaoController(IInstituicaoRepository instituicaoRepository){
            this.instituicaoRepository = instituicaoRepository;
        }

        // GET instituicao/{id}
        [HttpGet("{id}", Name = "getInstituicaoID")]
        public async Task<IActionResult> Get(int Id){

            ReturnRequest result = new ReturnRequest();

            try{
                if (Id > 0)
                    result.Data = await instituicaoRepository.GetById(Id);

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

        // POST instituicao
        /*[HttpPost]
        public async Task<IActionResult> Post([FromBody] Instituicao instituicao){

            ReturnRequest result = new ReturnRequest();

            try{
                result.data = instituicaoRepository.add(instituicao).Result;
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

        // PUT instituicao
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instituicao Instituicao){

            ReturnRequest result = new ReturnRequest();

            try{
                if (await instituicaoRepository.GetById(Instituicao.Nr_id) != null)
                    result.Data = await instituicaoRepository.Update(Instituicao);
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
