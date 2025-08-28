using Microsoft.AspNetCore.Mvc;
using MotosRental.DTOs;
using MotosRental.DTOs.Examples;
using MotosRental.Entities;
using MotosRental.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotosRental.Controllers;

[ApiController]
[Route("api/driver")]
[Produces("application/json")]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;

    public DriverController(IDriverService driverService)
    {
        _driverService = driverService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo motorista.")]
    [SwaggerRequestExample(typeof(DriverRequestDTO), typeof(DriverRequestExample))]
    [ProducesResponseType(typeof(DriverResponseDTO), 201)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 400)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 409)]
    public async Task<IActionResult> Create([FromBody] DriverRequestDTO dto)
    {
        var driver = await _driverService.CreateAsync(dto);
        return CreatedAtAction(nameof(FindById), new { id = driver.Id }, driver);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Retorna todos os motoristas.")]
    [ProducesResponseType(typeof(List<DriverResponseDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
    public async Task<IActionResult> FindAll()
    {
        var drivers = await _driverService.FindAllAsync();
        return Ok(drivers);
    }
    
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Retorna um motorista pelo seu identificador.")]
    [ProducesResponseType(typeof(DriverResponseDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
    public async Task<IActionResult> FindById(long id)
    {
        var driver = await _driverService.FindByIdAsync(id);
        return Ok(driver);
    }

    [HttpPatch("{id:long}/upload-license")]
    [SwaggerOperation(Summary = "Envia a imagem da CNH.")]
    [ProducesResponseType(typeof(DriverResponseDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 400)] 
    [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 409)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
    public async Task<IActionResult> UploadLicense(long id, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return BadRequest(new { error = "Nenhum arquivo foi enviado." });
        }

        var driver = await _driverService.UploadDriverLicenseAsync(id, imageFile);
        return Ok(driver);
    }
    
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deleta um motorista pelo seu identificador.")]
    [ProducesResponseType(204)] 
    [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
    [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
    public async Task<IActionResult> Delete(long id)
    {
        await _driverService.RemoveAsync(id);
        return NoContent();
    }
}
