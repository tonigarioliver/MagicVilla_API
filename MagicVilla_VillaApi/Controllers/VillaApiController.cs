using AutoMapper;
using Azure;
using MagicVilla_VillaApi.Core.IConfiguration;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogger<VillaApiController> _logger;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public VillaApiController(ILogger<VillaApiController>logger, IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _logger = logger;
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;

        }
        [HttpGet]
        [Route("GetAllVillas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetAllAsync()
        {
            _logger.LogInformation("Get Villas");
            return Ok(await _UnitOfWork.Villas.GetAllAsync());
        }

        [HttpGet("GetVillaById{id:int}",Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<VillaDTO>> GetVillaAsync(int id)
        {
            if(id == 0) 
            {
                _logger.LogError("Get Villa error with id" + id);
                return BadRequest(); 
            }
            var villa=await _UnitOfWork.Villas.GetAsync(x => x.Id == id);
            if (villa is null) { return NotFound(); }
            return Ok(villa);   
        }

        [HttpPost]
        [Route("AddVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<VillaDTO>> AddVilaAsync([FromBody] VillaCreateDTO createDTO)
        {
            if (await _UnitOfWork.Villas.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            var _mappedVilla = _mapper.Map<Villa>(createDTO);
            if(!TryValidateModel(_mappedVilla)) { return BadRequest(ModelState); }
            _mappedVilla.CreateDate = DateTime.Now;
            await _UnitOfWork.Villas.CreateAsync(_mappedVilla);
            await _UnitOfWork.CompleteAsyn();
            var result = _mapper.Map<VillaDTO>(_mappedVilla);
            return CreatedAtRoute("GetVillaById",new {Id= _mappedVilla.Id}, result);
        }
        
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteVilla(int id)
        {            
            if ((await _UnitOfWork.Villas.GetAsync(villa => villa.Id ==id,false)) is null)
            {
                return NotFound();
            }
           
            if (id.Equals(0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var villa = await _UnitOfWork.Villas.GetAsync(villa => villa.Id == id);
            await _UnitOfWork.Villas.Delete(villa);
            await _UnitOfWork.CompleteAsyn();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> UpdateVilla(int Id, VillaUpdateDTO villaDTO)
        //{
        //if(villaDTO == null || villaDTO.Id!=Id)
        //{
        //    return BadRequest();
        //}

        //if (await _UnitOfWork.Villas.GetAsync(villa => villa.Id == Id) is null)
        //{
        //    return NotFound();
        //}
        //var villa = await _UnitOfWork.Villas.GetAsync(villa => villa.Id == Id,false);
        //var _mappedVilla = _mapper.Map<Villa>(villaDTO);
        //_mappedVilla.UpdateDate = DateTime.Now;
        //await _UnitOfWork.Villas.Update(_mappedVilla);
        //await _UnitOfWork.CompleteAsyn();
        //return NoContent();
        //}
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            if((await _UnitOfWork.Villas.GetAsync(x => x.Id == id,false)) is null)
            {
                return NotFound();
            }
            var villa = await _UnitOfWork.Villas.GetAsync(villa => villa.Id == id, false);
            var _mappedVilla = _mapper.Map<Villa>(updateDTO);
            _mappedVilla.UpdateDate = DateTime.Now;
            await _UnitOfWork.Villas.Update(_mappedVilla);
            await _UnitOfWork.CompleteAsyn();
            return NoContent();
        }

        [HttpPatch("UpdatePartialVilla/{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdatePartialVila(int id, JsonPatchDocument<VillaUpdateDTO> villaPatch)
        {
            if (villaPatch == null || id == 0)
            {
                return BadRequest();
            }

            if (await _UnitOfWork.Villas.GetAsync(villa => villa.Id == id) is null)
            {
                return NotFound();
            }
            var villa = await _UnitOfWork.Villas.GetAsync(villa => villa.Id == id, false);
            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
            villaPatch.ApplyTo(villaDTO, ModelState);
            Villa model = _mapper.Map<Villa>(villaDTO);
            model.UpdateDate = DateTime.Now;
            //_mappedVilla.ApplyTo(villa, ModelState);
            await _UnitOfWork.Villas.Update(model);
            await _UnitOfWork.CompleteAsyn();
            return NoContent();
        }
    }
}
