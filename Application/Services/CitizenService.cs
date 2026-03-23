using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class CitizenService : ICitizenService
    {
        private readonly ICitizenRepository _citizenRepository;

        public CitizenService(ICitizenRepository citizenRepository)
        {
            _citizenRepository = citizenRepository;
        }

       public List<Citizen> GetCitizen()
        {
    
            return _citizenRepository.Get();
        }

        public void CreateCitizen(CreateCitizenDto dto)
        {
            // Validar que no existe un ciudadano con el mismo DNI
            var existingCitizen = _citizenRepository.GetCitizenByDni(dto.DNI);
            if (existingCitizen != null)
                throw new Exception($"Ya existe un ciudadano con DNI {dto.DNI}");

            var citizen = new Citizen
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Adress = dto.Adress,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _citizenRepository.AddCitizen(citizen);
        }

        public bool DeleteCitizen(int dni)
        {
            var existing = _citizenRepository.GetCitizenByDni(dni);
            if (existing == null) return false;

            _citizenRepository.DeleteCitizen(existing);
            return true;
        }

        public Citizen? GetCitizenByDni(int dni)
        {
            return _citizenRepository.GetCitizenByDni(dni);
        }

        public Citizen UpdateCitizen(int dni, UpdateCitizenDto dto)
        {
            var existing = _citizenRepository.GetCitizenByDni(dni);
            if (existing == null) throw new Exception("Ciudadano no encontrado");

            existing.Name = dto.Name;
            existing.LastName = dto.LastName;
            existing.Adress = dto.Adress;
            existing.Phone = dto.Phone;
            existing.Email = dto.Email;

            return _citizenRepository.UpdateCitizen(existing);
        }
    }
}
