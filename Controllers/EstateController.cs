using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Estates;
using NetKubernetes.Dtos.EstateDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;

namespace NetKubernetes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstateController : ControllerBase
{
  private readonly IEstateRepository _repository;
  private IMapper _mapper;

  public EstateController(IEstateRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [HttpGet]
  public ActionResult<IEnumerable<EstateResponseDto>> GetAllEstates()
  {
    var estates = _repository.GetAllEstates();
    return Ok(_mapper.Map<IEnumerable<EstateResponseDto>>(estates));
  }

  [HttpGet("{id}", Name = "GetEstateById")]
  public ActionResult<EstateResponseDto> GetEstateById(int id)
  {
    var estate = _repository.GetEstateById(id);
    if (estate is null)
      throw new MiddlewareException(HttpStatusCode.NotFound, new { message = $"Estate with the id {id} not found" });

    return Ok(_mapper.Map<EstateResponseDto>(estate));
  }

  [HttpPost]
  public ActionResult<EstateResponseDto> CreateEstate([FromBody] EstateRequestDto estate)
  {
    var estateModel = _mapper.Map<Estate>(estate);
    _repository.CreateEstate(estateModel);
    _repository.SaveChanges();

    var estateResponseDto = _mapper.Map<EstateResponseDto>(estateModel);

    return CreatedAtRoute(nameof(GetEstateById), new { estateResponseDto.Id }, estateResponseDto);
  }

  [HttpDelete("{id}")]
  public ActionResult DeleteEstate(int id)
  {
    var estate = _repository.GetEstateById(id);
    if (estate is null)
      throw new MiddlewareException(HttpStatusCode.NotFound, new { message = $"Estate with the id {id} not found" });

    _repository.DeleteEstate(id);
    _repository.SaveChanges();

    return NoContent();
  }
}