using MagicVilla_VillaApi.Core.IConfiguration;
using MagicVilla_VillaApi.Core.IRepository;
using MagicVilla_VillaApi.Core.Repository;

namespace MagicVilla_VillaApi.Data
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;
        public VillaRepository Villas { get; private set; }

        public UnitOfWork(AppDbContext appDbContext,ILogger logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            Villas = new VillaRepository(appDbContext,logger);
        }


        public async Task CompleteAsyn()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
