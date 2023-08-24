using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        [HttpGet]
        [Route("GetAllVillas")]
        public ActionResult<IEnumerable<VillaDTO>> GetAll()
        {
            return VillaStore.VillasList;
        }
        [HttpGet]
        [Route("GetVillaById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id == 0) { return BadRequest(); }
            var villa=VillaStore.VillasList.FirstOrDefault(x => x.Id == id);
            if (villa is null) { return NotFound(); }
            return Ok(villa);   
        }
    }
}
