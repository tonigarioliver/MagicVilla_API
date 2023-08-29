using MagicVilla_VillaApi.Core.IConfiguration;
using MagicVilla_VillaApi.Core.IRepository;
using MagicVilla_VillaApi.Core.Repository;

namespace MagicVilla_VillaApi.Data
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppDbContext _appDbContext;
        public VillaRepository Villas { get; private set; }

        public ILogger<IUnitOfWork> logger { get; private set; }

        public UnitOfWork(AppDbContext appDbContext,ILogger<IUnitOfWork> logger)
        {
            _appDbContext = appDbContext;
            this.logger = logger;
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
