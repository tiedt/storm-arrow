using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("pesquisaavancada")]
    public class PesquisaAvancadaController : Controller{

        protected readonly IPropostaRepository propostaRepository;

        public PesquisaAvancadaController(IPropostaRepository propostaRepository){
            this.propostaRepository = propostaRepository;
        }

        // GET pesquisaavancada?dt_geracao={dt_geracao}&ds_tipo={ds_tipo}&nr_id_curso={nr_id_curso}&qt_participantes={qt_participantes}&ds_nome_ds_desc_projeto={ds_nome_ds_desc_projeto}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime? dt_geracaoIni, DateTime? dt_geracaoFim, string? ds_tipo, int? nr_id_curso, int? qt_participantes, string? ds_nome_ds_desc_projeto){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await propostaRepository.GetByParams(dt_geracaoIni, dt_geracaoFim,ds_tipo, nr_id_curso, qt_participantes, ds_nome_ds_desc_projeto);

                if (result.Data != null
                && ((List<Proposta>) result.Data).Count > 0){
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
    }
}
