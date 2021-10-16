using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Controllers{

    [ApiController]
    [Route("estoque/atualizar")]
    public class EstoqueController : Controller{

        protected readonly IEstoqueRepository estoqueRepository;

        public EstoqueController(IEstoqueRepository estoqueRepository){
            this.estoqueRepository = estoqueRepository;
        }

        // GET estoque/atualizar
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int Id_produto, [FromQuery] float Saldo){

            ReturnRequest result = new ReturnRequest();

            try{
                result.Data = await estoqueRepository.Update(new Estoque(){ Id_produto = Id_produto, Saldo = Saldo });

                if (result.Data != null
                && ((bool) result.Data)){
                    result.Status = "200"; // OK
                    return Ok(result);
                }else{
                    result.Status = "409";
                    result.Data = null;
                    return Conflict(result);
                }
            }catch (Exception){
                result.Status = "409";
                result.Data = null;
                return Conflict(result);
            }
        }
    }
}
