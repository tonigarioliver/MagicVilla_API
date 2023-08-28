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

        public UnitOfWork(AppDbContext appDbContext,ILoggerFactory logger)
        {
            _appDbContext = appDbContext;
            _logger = logger.CreateLogger("Logs");
            Villas = new VillaRepository(appDbContext,_logger);
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
