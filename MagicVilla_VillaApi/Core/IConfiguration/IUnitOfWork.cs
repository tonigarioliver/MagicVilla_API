using MagicVilla_VillaApi.Core.IRepository;
using MagicVilla_VillaApi.Core.Repository;

namespace MagicVilla_VillaApi.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        public VillaRepository Villas { get; }
        Task CompleteAsyn();
    }
}
