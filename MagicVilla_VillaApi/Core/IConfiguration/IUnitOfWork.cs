using MagicVilla_VillaApi.Core.IRepository;
using MagicVilla_VillaApi.Core.Repository;

namespace MagicVilla_VillaApi.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        public VillaRepository Villas { get; }
        public ILogger<IUnitOfWork> logger { get; }
        Task CompleteAsyn();
    }
}
