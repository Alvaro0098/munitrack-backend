using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepository _operatorRepository;

        public OperatorService(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        public List<Operator> GetOperators()
        {
            return _operatorRepository.GetOperators();
        }
        public void CreateOperator(CreateOperatorDto Dto)
        {
            var existingOperator = _operatorRepository.GetOperatorByDni(Dto.DNI);
            if (existingOperator != null)
                throw new Exception($"Ya existe un operador con DNI {Dto.DNI}");

            Operator newOperator = new Operator()
            {
                DNI = Dto.DNI,
                Name = Dto.Name,
                LastName = Dto.LastName,
                NLegajo = Dto.NLegajo,
                Password = Dto.Password,
                Phone = Dto.Phone,
                Email = Dto.Email,
                Position = Dto.Position
            };
            _operatorRepository.AddOperator(newOperator);
        }

        public bool DeleteOperator(int dni)
        {
            var operatorDelete = _operatorRepository.GetOperatorByDni(dni);
            if (operatorDelete is null)
                return false;
                
            _operatorRepository.DeleteOperator(operatorDelete);
            return true;
        }

        public Operator? GetOperatorByDni(int dni)
        {
            return _operatorRepository.GetOperatorByDni(dni);
        }

        public Operator UpdateOperator(int dni, UpdateOperatorDto Dto)
        {
            var operatorEntity = _operatorRepository.GetOperatorByDni(dni);
            if (operatorEntity is null)
                throw new Exception("Operator no encontrado");

            operatorEntity.Name = Dto.Name;
            operatorEntity.LastName = Dto.LastName;
            
            operatorEntity.Password = Dto.Password;
            operatorEntity.Phone = Dto.Phone;
            operatorEntity.Email = Dto.Email;
            operatorEntity.Position = Dto.Position;

            _operatorRepository.UpdateOperator(operatorEntity);
            return operatorEntity;               
        }
        public Operator? ValidateOper(AuthenticationDTO authDTO)
        {
            // Usa el método heredado del repositorio para validar el usuario.
            return _operatorRepository.GetUserByNLegajoAndPassword(authDTO.NLegajo, authDTO.Password);
        }
    }
}
