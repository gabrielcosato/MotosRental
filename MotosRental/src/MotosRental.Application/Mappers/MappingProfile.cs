using System.Text.RegularExpressions;
using AutoMapper;
using MotosRental.DTOs;
using MotosRental.Entities;

namespace MotosRental.MotosRental.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<DriverRequestDTO, Driver>().ForMember(
            dest => dest.Cnpj, 
            opt => opt.MapFrom(src => CleanCnpj(src.Cnpj)) 
        );
      
        CreateMap<Driver, DriverResponseDTO>();
        
        
        CreateMap<MotorcycleRequestDTO, Motorcycle>();
        
        CreateMap<Motorcycle, MotorcycleResponseDTO>();
    }
    
    private string CleanCnpj(string cnpj)
    {
        if (string.IsNullOrEmpty(cnpj))
            return string.Empty;
        
        return Regex.Replace(cnpj, @"[^\d]", "");
    }
  
}