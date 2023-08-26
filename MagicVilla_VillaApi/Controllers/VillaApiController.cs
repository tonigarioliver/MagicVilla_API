using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogger<VillaApiController> _logger;

        public VillaApiController(ILogger<VillaApiController>logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("GetAllVillas")]
        public ActionResult<IEnumerable<VillaDTO>> GetAll()
        {
            _logger.LogInformation("Get Villas");
            return VillaStore.VillasList;
        }

        [HttpGet("{id:int}",Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id == 0) 
            {
                _logger.LogError("Get Villa error with id" + id);
                return BadRequest(); }
            var villa=VillaStore.VillasList.FirstOrDefault(x => x.Id == id);
            if (villa is null) { return NotFound(); }
            return Ok(villa);   
        }

        [HttpPost]
        [Route("AddVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<VillaDTO> AddVila([FromBody]VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid) { return BadRequest(ModelState); }
            if(VillaStore.VillasList.Any(villa=>villa.Id == villaDTO.Id)) {
                ModelState.AddModelError("", "Villa Name Already Exists");
                return BadRequest(); 
            }
            if (villaDTO is null) { return BadRequest(); }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.VillasList.OrderByDescending(villa => villa.Id)
                                            .FirstOrDefault().Id + 1;
            VillaStore.VillasList.Add(villaDTO);
            return CreatedAtRoute("GetVillaById",new {Id=villaDTO.Id},villaDTO);
        }

        [HttpDelete("{id:int}",Name ="DeleteVillaById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteVillaById(int Id)
        {            
            if (!VillaStore.VillasList.Any(villa => villa.Id ==Id))
            {
                return NotFound();
            }
           
            if (Id.Equals(0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var villa = VillaStore.VillasList.FirstOrDefault(villa => villa.Id == Id);
            VillaStore.VillasList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateVillaById(int Id, [FromBody] VillaDTO villaDTO)
        {
            if(villaDTO == null || villaDTO.Id!=Id)
            {
                return BadRequest();
            }
            
            if (!VillaStore.VillasList.Any(villa => villa.Id == Id))
            {
                return NotFound();
            }
            var villa = VillaStore.VillasList.FirstOrDefault(villa => villa.Id == Id);
            villa.Name= villaDTO.Name;
            villa.Occupancy=villaDTO.Occupancy;
            villa.Sqft=villaDTO.Sqft;
            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdatePartialVila(int Id, JsonPatchDocument<VillaDTO> villaPatch)
        {
            if (villaPatch == null || Id == 0)
            {
                return BadRequest();
            }

            if (!VillaStore.VillasList.Any(villa => villa.Id == Id))
            {
                return NotFound();
            }
            var villa = VillaStore.VillasList.FirstOrDefault(villa => villa.Id == Id);
            villaPatch.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
