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
  public async Task<ActionResult<IEnumerable<EstateResponseDto>>> GetAllEstates()
  {
    var estates = await _repository.GetAllEstates();
    return Ok(_mapper.Map<IEnumerable<EstateResponseDto>>(estates));
  }

  [HttpGet("{id}", Name = "GetEstateById")]
  public async Task<ActionResult<EstateResponseDto>> GetEstateById(int id)
  {
    var estate = await _repository.GetEstateById(id);
    if (estate is null)
      throw new MiddlewareException(HttpStatusCode.NotFound, new { message = $"Estate with the id {id} not found" });

    return Ok(_mapper.Map<EstateResponseDto>(estate));
  }

  [HttpPost]
  public async Task<ActionResult<EstateResponseDto>> CreateEstate([FromBody] EstateRequestDto estate)
  {
    var estateModel = _mapper.Map<Estate>(estate);
    await _repository.CreateEstate(estateModel);
    await _repository.SaveChanges();

    var estateResponseDto = _mapper.Map<EstateResponseDto>(estateModel);

    return CreatedAtRoute(nameof(GetEstateById), new { estateResponseDto.Id }, estateResponseDto);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteEstate(int id)
  {
    var estate = await _repository.GetEstateById(id);
    if (estate is null)
      throw new MiddlewareException(HttpStatusCode.NotFound, new { message = $"Estate with the id {id} not found" });

    await _repository.DeleteEstate(id);
    await _repository.SaveChanges();

    return NoContent();
  }
}