using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotosRental.DTOs;
using MotosRental.Entities;
using MotosRental.Enums;
using MotosRental.Exceptions;
using MotosRental.Interfaces;

namespace MotosRental.Services;

public class DriverService : IDriverService
{
    private readonly IDriverRepository _driverRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public DriverService(IDriverRepository driverRepository, IWebHostEnvironment env, IMapper mapper)
    {
        _driverRepository = driverRepository;
        _env = env;
        _mapper = mapper;
    }

    public async Task<DriverResponseDTO> CreateAsync(DriverRequestDTO dto)
    {

        var cleanedCnpj = CleanCnpj(dto.Cnpj);
        if (!IsCnpjValid(cleanedCnpj))
        {
            throw new BusinessValidationException("Formato de CNPJ inválido.");
        }


        if (!IsDriverLicenseTypeValid(dto.DriverLicenseType))
        {
            throw new BusinessValidationException("Tipo de CNH inválido. Tipos aceitos: A, B ou Ab.");
        }


        if (!IsDriverLicense(dto.DriverLicenseNumber))
        {
            throw new BusinessValidationException("Formato de CNH inválido.");
        }
        
        
        if (await _driverRepository.CnpjExistsAsync(dto.Cnpj))
        {
            throw new DuplicateDataException("O CNPJ informado já está cadastrado.");
        }


        if (await _driverRepository.EmailExistsAsync(dto.Email))
        {
            throw new DuplicateDataException("O Email informado já está cadastrado.");
        }


        if (await _driverRepository.DriverLicenseNumberExistsAsync(dto.DriverLicenseNumber))
        {
            throw new DuplicateDataException("O número da CNH informado já está cadastrado.");
        }
        
        var driver = _mapper.Map<Driver>(dto);

        await _driverRepository.CreateAsync(driver);
        await _driverRepository.SaveChangesAsync();

        return _mapper.Map<DriverResponseDTO>(driver);
    }

    public async Task<DriverResponseDTO> UploadDriverLicenseAsync(long id, IFormFile imageFile)
    {
        var driver = await FindByIdAsyncEntity(id); 

        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (extension != ".png" && extension != ".bmp")
        {
            throw new BusinessValidationException("Formato de imagem inválido. Apenas .png e .bmp são aceitos.");
        }
            

        var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "cnh");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
            

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        driver.DriverLicenseImage = $"/uploads/cnh/{fileName}";
        await _driverRepository.SaveChangesAsync();

        return _mapper.Map<DriverResponseDTO>(driver);
    }

    public async Task<List<DriverResponseDTO>> FindAllAsync()
    {
        var drivers = await _driverRepository.FindAllAsync();
        return _mapper.Map<List<DriverResponseDTO>>(drivers);
    }

    public async Task<DriverResponseDTO> FindByIdAsync(long id)
    {
        var driver = await FindByIdAsyncEntity(id); 

        return _mapper.Map<DriverResponseDTO>(driver);
    }
    
    private async Task<Driver> FindByIdAsyncEntity(long id)
    {
        var driver = await _driverRepository.FindByIdAsync(id);
            
        return driver ?? throw new KeyNotFoundException("Motorista não encontrado para o ID informado.");
          
    }
    
    public async Task RemoveAsync(long id)
    {
        var driver = await FindByIdAsyncEntity(id); 
        
        _driverRepository.RemoveAsync(driver);
        await _driverRepository.SaveChangesAsync();
    }

    #region Private Validations
    private string CleanCnpj(string cnpj) => Regex.Replace(cnpj, @"[^\d]", "");

    private bool IsCnpjValid(string cnpj)
    {
        if (cnpj.Length != 14 || new string(cnpj[0], 14) == cnpj)
        {
            return false;
        }
        
        try
        {
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            if (cnpj.Length != 14)
            {
                return false;
            }
                

            int[] multipliers1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipliers2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(cnpj[i].ToString()) * multipliers1[i];
            }
            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

         
            sum = 0;
            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(cnpj[i].ToString()) * multipliers2[i];
            }
            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return digit1 == int.Parse(cnpj[12].ToString()) &&
                   digit2 == int.Parse(cnpj[13].ToString());
        }
        catch
        {
            return false;
        }
    }
    
    private bool IsDriverLicense(string licenseNumber)
    {
        try
        {
            licenseNumber = Regex.Replace(licenseNumber, "[^0-9]", "");
            if (licenseNumber.Length != 11)
            {
                return false;
            }
            
            long.Parse(licenseNumber);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool IsDriverLicenseTypeValid(LicenseType type)
    {
        return type == LicenseType.A || type == LicenseType.B || type == LicenseType.Ab;
    }
    #endregion
}
