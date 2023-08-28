using MagicVilla_VillaApi.Core.IRepository;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;

namespace MagicVilla_VillaApi.Core.Repository
{
    public class VillaRepository : Generic<Villa>
    {
        public VillaRepository(AppDbContext appDbContext, ILogger logger) : base(appDbContext, logger)
        {
        }
    }
}
