using Microsoft.AspNetCore.Mvc;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Controllers{
    
    [ApiController]
    [Route("dragrace")]
    public class DragRaceController : Controller{

        protected readonly IDragRaceRepository dragRaceRepository;

        public DragRaceController(IDragRaceRepository dragRaceRepository){
            this.dragRaceRepository = dragRaceRepository;
        }

        // GET dragrace
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int VitCar1, [FromQuery] int VitCar2, [FromQuery] int VitCar3, [FromQuery] int VitCar4){

            ReturnRequest result = new ReturnRequest();

            try{
                
                result.Data = await dragRaceRepository.Insert(new DragRace() {VitCar1 = VitCar1, VitCar2 = VitCar2, VitCar3 = VitCar3, VitCar4 = VitCar4});

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
