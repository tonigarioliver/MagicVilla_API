using AutoMapper;
using MagicVilla_VillaApi.Models.DTOs;

namespace MagicVilla_VillaApi.Models.Profiles
{
    public class VillaProfile:Profile
    {
        public VillaProfile()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }
    }
}
