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
    }
}
