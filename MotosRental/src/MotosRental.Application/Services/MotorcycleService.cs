using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotosRental.DTOs;
using MotosRental.Entities;
using MotosRental.Exceptions;
using MotosRental.Interfaces;
using MotosRental.MotosRental.Application.Events;

namespace MotosRental.Services;

 public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _motorcycleRepository = motorcycleRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<MotorcycleResponseDTO> CreateAsync(MotorcycleRequestDTO dto)
        {
            if (!IsPlateValid(dto.LicensePlate))
            {
                throw new InvalidLicensePlateException("Formato de placa inválido.");
            }

            if (await _motorcycleRepository.LicensePlateExistsAsync(dto.LicensePlate))
            {
                throw new DuplicateLicensePlateException("A placa informada já está cadastrada no sistema.");
            }
            
            var motorcycle = _mapper.Map<Motorcycle>(dto);

            await _motorcycleRepository.CreateAsync(motorcycle);
            await _motorcycleRepository.SaveChangesAsync(); 
            
            var ev = new MotorcycleCreatedEvent
            {
                Id = motorcycle.Id,
                LicensePlate = motorcycle.LicensePlate,
                Year = motorcycle.Year,
                Model = motorcycle.Model,
                CreatedAt = DateTime.UtcNow
            };
            
            await _messagePublisher.PublishAsync("motorcycle.events", "motorcycle.created", ev);

            return _mapper.Map<MotorcycleResponseDTO>(motorcycle);
        }

        public async Task<List<MotorcycleResponseDTO>> FindAllAsync()
        {
            var motorcycles = await _motorcycleRepository.FindAllAsync();
            return _mapper.Map<List<MotorcycleResponseDTO>>(motorcycles);
        }
        
        public async Task<MotorcycleResponseDTO> FindByPlateAsync(string plate)
        {
            
            if (!IsPlateValid(plate))
            {
                throw new InvalidLicensePlateException("Formato de placa inválido.");
            }

            if (!await _motorcycleRepository.LicensePlateExistsAsync(plate))
            {
                throw new KeyNotFoundException("Moto não encontrada para a placa informada.");
            }
            
            var motorcycle = await _motorcycleRepository.FindByPlateAsync(plate);
            
            return _mapper.Map<MotorcycleResponseDTO>(motorcycle);
        }
        
        public async Task<MotorcycleResponseDTO> FindByIdAsync(long id)
        {
            var motorcycle = await FindByIdAsyncEntity(id); 
            
            return _mapper.Map<MotorcycleResponseDTO>(motorcycle);
        }
        
        private async Task<Motorcycle> FindByIdAsyncEntity(long id)
        {
            var motorcycle = await _motorcycleRepository.FindByIdAsync(id);
            
            return motorcycle ?? throw new KeyNotFoundException("Moto não encontrada para o ID informado.");
          
        }

        
        public async Task<MotorcycleResponseDTO> UpdatePlateAsync(long id, MotorcycleUpdatePlateRequestDTO dto)
        {
            var motorcycle = await FindByIdAsyncEntity(id); 

            if (!IsPlateValid(dto.LicensePlate))
            {
                throw new InvalidLicensePlateException("Formato de placa inválido.");
            }
            
            
            if (motorcycle.LicensePlate != dto.LicensePlate && await _motorcycleRepository.LicensePlateExistsAsync(dto.LicensePlate))
            {
                throw new DuplicateLicensePlateException("A nova placa informada já pertence a outra moto.");
            }

            motorcycle.LicensePlate = dto.LicensePlate;
            await _motorcycleRepository.SaveChangesAsync();

            return _mapper.Map<MotorcycleResponseDTO>(motorcycle);
        }
        
        public async Task RemoveAsync(long id)
        {
            var motorcycle = await FindByIdAsyncEntity(id);
            
            _motorcycleRepository.RemoveAsync(motorcycle);
            await _motorcycleRepository.SaveChangesAsync();
        }

        #region Private Validations
        private bool IsPlateValid(string plate)
        {
            var commonPattern = "^[A-Z]{3}-?\\d{4}$";
            var mercosulPattern = "^[A-Z]{3}\\d[A-Z]{1}\\d{2}$"; 
            
            return System.Text.RegularExpressions.Regex.IsMatch(plate.ToUpper(), commonPattern) ||
                   System.Text.RegularExpressions.Regex.IsMatch(plate.ToUpper(), mercosulPattern);
        }
        #endregion
    }