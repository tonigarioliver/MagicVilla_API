﻿using AutoMapper;
using Azure;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogger<VillaApiController> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VillaApiController(ILogger<VillaApiController>logger,AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;

        }
        [HttpGet]
        [Route("GetAllVillas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetAllAsync()
        {
            _logger.LogInformation("Get Villas");
            return Ok(await _context.Villas.ToListAsync());
        }

        [HttpGet("{id:int}",Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<VillaDTO>> GetVillaAsync(int id)
        {
            if(id == 0) 
            {
                _logger.LogError("Get Villa error with id" + id);
                return BadRequest(); }
            var villa=await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);
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
            //if(!ModelState.IsValid) { return BadRequest(ModelState); }
            if (await _context.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            var _mappedVilla = _mapper.Map<Villa>(createDTO);
            _mappedVilla.CreateDate = DateTime.Now;
            _context.Villas.Add(_mappedVilla);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<VillaDTO>(_mappedVilla);
            return CreatedAtRoute("GetVillaById",new {Id= _mappedVilla.Id}, result);
        }

        [HttpDelete("{id:int}",Name ="DeleteVillaById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteVillaByIdAsync(int Id)
        {            
            if (!await _context.Villas.AnyAsync(villa => villa.Id ==Id))
            {
                return NotFound();
            }
           
            if (Id.Equals(0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var villa = await _context.Villas.FirstOrDefaultAsync(villa => villa.Id == Id);
            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateVillaById(int Id, [FromBody] VillaDTO villaDTO)
        {
            if(villaDTO == null || villaDTO.Id!=Id)
            {
                return BadRequest();
            }
            
            if (!await _context.Villas.AnyAsync(villa => villa.Id == Id))
            {
                return NotFound();
            }
            var villa = await _context.Villas.AsTracking().FirstOrDefaultAsync(villa => villa.Id == Id);
            var _mappedVilla = _mapper.Map<Villa>(villaDTO);
            _mappedVilla.UpdateDate = DateTime.Now;
            _context.Villas.Update(_mappedVilla);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdatePartialVila(int Id, JsonPatchDocument<VillaUpdateDTO> villaPatch)
        {
            if (villaPatch == null || Id == 0)
            {
                return BadRequest();
            }

            if (!await _context.Villas.AnyAsync(villa => villa.Id == Id))
            {
                return NotFound();
            }
            var villa = await _context.Villas.AsTracking().FirstOrDefaultAsync(villa => villa.Id == Id);
            var _mappedVilla = _mapper.Map<Villa>(villaPatch);
            _mappedVilla.UpdateDate = DateTime.Now;
            _context.Villas.Update(_mappedVilla);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
