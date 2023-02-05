using AutoMapper;
using NetKubernetes.Dtos.EstateDtos;
using NetKubernetes.Models;

namespace NetKubernetes.Profiles;

public class EstateProfile : Profile
{
  public EstateProfile()
  {
    CreateMap<Estate, EstateResponseDto>();
    CreateMap<EstateRequestDto, Estate>();
  }
}