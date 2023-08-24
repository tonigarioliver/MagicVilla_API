using MagicVilla_VillaApi.Models.DTOs;

namespace MagicVilla_VillaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> VillasList=new List<VillaDTO>
                        {
                            new VillaDTO{Id = 1,Name = "Test1"},
                            new VillaDTO { Id = 2, Name = "Test2"}
                        };
    }
}
