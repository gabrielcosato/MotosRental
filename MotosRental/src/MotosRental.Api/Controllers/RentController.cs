using Microsoft.AspNetCore.Mvc;
using MotosRental.DTOs;
using MotosRental.DTOs.Examples;
using MotosRental.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotosRental.Controllers;

[ApiController]
[Route("api/rent")]
[Produces("application/json")]
public class RentController : ControllerBase
{
    private readonly IRentService _rentService;

    public RentController(IRentService rentService)
    {
        _rentService = rentService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um aluguel de moto.")]
    [SwaggerRequestExample(typeof(RentRequestDTO), typeof(RentRequestExample))]
    [ProducesResponseType(typeof(RentResponseDTO), 201)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 400)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
    public async Task<IActionResult> Create([FromBody] RentRequestDTO dto)
    {
        var rentResponse = await _rentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = rentResponse.Id }, rentResponse);
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Busca um aluguel pelo identificador.")]
    [ProducesResponseType(typeof(RentResponseDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
    public async Task<IActionResult> GetById(long id)
    {
        var rentResponse = await _rentService.GetByIdAsync(id);
        return Ok(rentResponse);
    }

    [HttpPut("{id:long}/devolucao")]
    [SwaggerOperation(Summary = "Registra a data de devolução da moto no aluguel.")]
    [ProducesResponseType(typeof(RentResponseDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 400)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
    public async Task<IActionResult> Return(long id, [FromBody] ReturnRentRequestDTO dto)
    {
        var rentResponse = await _rentService.ProcessReturnAsync(id, dto);
        return Ok(rentResponse);
    }
    
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deleta um aluguel pelo seu identificador.")]
    [ProducesResponseType(204)] 
    [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
    public async Task<IActionResult> Delete(long id)
    {
        await _rentService.RemoveAsync(id);
        return NoContent();
    }
}