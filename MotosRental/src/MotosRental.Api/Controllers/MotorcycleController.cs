using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotosRental.DTOs;
using MotosRental.DTOs.Examples;
using MotosRental.Entities;
using MotosRental.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotosRental.Controllers;

[ApiController]
[Route("api/motorcycle")]
[Authorize]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService;
        
        public MotorcyclesController(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }
  
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [SwaggerOperation(Summary = "Cria uma nova moto (Admin)")]
        [SwaggerRequestExample(typeof(MotorcycleRequestDTO), typeof(MotorcycleRequestExample))]
        [ProducesResponseType(typeof(MotorcycleResponseDTO), 201)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 400)] 
        [ProducesResponseType(typeof(ErrorResponseDTO), 409)] 
        [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
        public async Task<IActionResult> Create([FromBody] MotorcycleRequestDTO dto)
        {
            var motorcycle = await _motorcycleService.CreateAsync(dto);
            return CreatedAtAction(nameof(FindById), new { id = motorcycle.Id }, motorcycle);
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna todas as motos")]
        [ProducesResponseType(typeof(List<MotorcycleResponseDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
        public async Task<IActionResult> FindAll()
        {
            var motos = await _motorcycleService.FindAllAsync();
            return Ok(motos);
        }
        
        [HttpGet("plate/{plate}")]
        [Authorize(Policy = "Admin")]
        [SwaggerOperation(Summary = "Retorna uma moto pela sua placa. (Admin)")]
        [ProducesResponseType(typeof(List<MotorcycleResponseDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
        public async Task<IActionResult> FindByPlate(string plate)
        {
            var motorcycle = await _motorcycleService.FindByPlateAsync(plate);
            return Ok(motorcycle);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna uma moto pelo seu identificador.")]
        [ProducesResponseType(typeof(MotorcycleResponseDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
        public async Task<IActionResult> FindById(long id)
        {
            var moto = await _motorcycleService.FindByIdAsync(id);
            return Ok(moto);
        }
        
        
        [HttpPatch("{id:long}/plate")] 
        [Authorize(Policy = "Admin")]
        [SwaggerOperation(Summary = "Edita a placa de uma moto. (Admin)")]
        [ProducesResponseType(typeof(Motorcycle), 200)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 400)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 409)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
        public async Task<IActionResult> UpdatePlate(long id, [FromBody] MotorcycleUpdatePlateRequestDTO dto)
        {
            var motorcycle = await _motorcycleService.UpdatePlateAsync(id, dto);
            return Ok(motorcycle);
        }

        [HttpDelete("{id:long}")]
        [Authorize(Policy = "Admin")]
        [SwaggerOperation(Summary = "Deleta uma moto sem alugueis associados pelo seu identificador. (Admin)")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(typeof(ErrorResponseDTO), 404)]
        [ProducesResponseType(typeof(ErrorResponseDTO), 500)]
        public async Task<IActionResult> Delete(long id)
        {
            await _motorcycleService.RemoveAsync(id);
            return NoContent();
        }
    }